using BlazorPlugin2.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Runtime.Loader;
using System.Xml;

namespace BlazorPlugin2.Client;

public class PackageRepository(HttpClient http, NavigationManager myNavigationManager)
    : IPackageRepository
{
    private List<Package> _packages = [];

    public async Task<List<Package>> GetList()
    {
        if (_packages.Count == 0)
        {
            var result = await http.GetFromJsonAsync<List<Package>>("/ModuleManager");
            if (result != null)
                _packages = result;
        }

        return _packages;
    }

    public bool CheckLoaded(string package)
    {
        return _packages.Any(s => s.Name == package && s.IsLoaded);
    }

    public async Task Upload(IBrowserFile file)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(file.OpenReadStream()), "file", file.Name);

        try
        {
            var response = await http.PostAsync("/ModuleManager", content);
            var _ = await response.Content.ReadAsStringAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task<bool> Load(Package package)
    {
        if(package.Name != null && CheckLoaded(package.Name)) return true;

        try
        {
            var stream = await http.GetStreamAsync($"{myNavigationManager.BaseUri}/_content/{package.Name}/{package.Name}.dll");
            var assembly = AssemblyLoadContext.Default.LoadFromStream(stream);
            package.Assembly = assembly;
            try
            {
                var stream2 = await http.GetStreamAsync($"{myNavigationManager.BaseUri}/_content/{package.Name}/{package.Name}.pdb");
                var symbols = AssemblyLoadContext.Default.LoadFromStream(stream2);
                package.Symbols = symbols;
            }
            catch
            {
                Console.WriteLine($"No symbols loaded for {package.Name}");
            }
            package.Components = assembly.GetExportedTypes().Select(s => (s.FullName ?? "", s.BaseType?.Name ?? "")).ToList();
            package.IsLoaded = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        // Find List of assets to load
        var stream3 = await http.GetStreamAsync($"{myNavigationManager.BaseUri}/_content/{package.Name}/Microsoft.AspNetCore.StaticWebAssets.props");
        XmlDocument assetsList = new XmlDocument();
        assetsList.Load(stream3);
        foreach (XmlNode asset in assetsList.GetElementsByTagName("StaticWebAsset"))
        {
            var content = asset.SelectSingleNode("RelativePath")?.InnerText;

            if (content is null)
                continue;
            
            if (content.EndsWith(".js"))
            {
                package.Assets.Add(("js", content));
            }
            else if (content.EndsWith(".css"))
            {
                package.Assets.Add(("css", content));
            }
        }

        return true;
    }
}
