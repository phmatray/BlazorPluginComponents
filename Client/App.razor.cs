using BlazorPlugin2.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorPlugin2.Client;

public partial class App
{
    private List<Package> _packages = [];
    
    [Inject]
    public required IPackageRepository Repo { get; set; }
    
    [Inject]
    public required Interop DOMInterop { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _packages = await Repo.GetList();
        foreach (var package in _packages)
        {
            await Repo.Load(package);
            foreach (var asset in package.Assets)
            {
                var id = $"{package.Name}{asset.Item2[..asset.Item2.LastIndexOf('.')]}";

                switch (asset.Item1)
                {
                    case "css":
                        await DOMInterop.IncludeLink(id, $"/_content/{package.Name}/{asset.Item2}");
                        break;
                    case "js":
                        await DOMInterop.IncludeScript(id, $"/_content/{package.Name}/{asset.Item2}");
                        break;
                }
            }
        }
    }
}