using System.Net.Http.Json;
using System.Runtime.Loader;
using System.Xml;
using BlazorPlugin2.Shared;
using BlazorPlugin2.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorPlugin2.Client.Services;

/// <summary>
/// Manages the operations related to package repositories.
/// </summary>
/// <param name="http">The HttpClient used for HTTP requests.</param>
/// <param name="navigationManager">The NavigationManager used for URL resolution.</param>
public class PackageRepository(
    HttpClient http,
    NavigationManager navigationManager)
    : IPackageRepository
{
    private const string ModuleManagerUrl = "/module-manager";
    
    private List<Package> _packages = [];

    /// <inheritdoc/>
    public async Task<List<Package>> GetList()
    {
        if (_packages.Count != 0)
        {
            return _packages;
        }

        try
        {
            var response = await http.GetFromJsonAsync<CollectionResponse<Package>>(ModuleManagerUrl);
            _packages = response?.Items ?? [];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return _packages;
    }

    /// <inheritdoc/>
    public bool CheckLoaded(string package)
    {
        return _packages.Any(s => s.Name == package && s.IsLoaded);
    }

    /// <inheritdoc/>
    public async Task Upload(IBrowserFile file)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(file.OpenReadStream()), "file", file.Name);

        try
        {
            var response = await http.PostAsync(ModuleManagerUrl, content);
            var _ = await response.Content.ReadAsStringAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> Load(Package package)
    {
        if(package.Name != null && CheckLoaded(package.Name))
        {
            return true;
        }

        try
        {
            var assemblyStream = await http.GetStreamAsync($"{navigationManager.BaseUri}/_content/{package.Name}/{package.Name}.dll");
            var assembly = AssemblyLoadContext.Default.LoadFromStream(assemblyStream);
            package.LoadAssembly(assembly);
            
            try
            {
                var symbolsStream = await http.GetStreamAsync($"{navigationManager.BaseUri}/_content/{package.Name}/{package.Name}.pdb");
                var symbols = AssemblyLoadContext.Default.LoadFromStream(symbolsStream);
                package.LoadSymbols(symbols);
            }
            catch
            {
                Console.WriteLine($"No symbols loaded for {package.Name}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        // Find List of assets to load
        var assetsStream = await http.GetStreamAsync($"{navigationManager.BaseUri}/_content/{package.Name}/Microsoft.AspNetCore.StaticWebAssets.props");
        XmlDocument assetsList = new XmlDocument();
        assetsList.Load(assetsStream);
        package.ParseAssetDetailsFromXml(assetsList);

        return true;
    }
}
