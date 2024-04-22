using BlazorPlugin2.Client.Services;
using BlazorPlugin2.Shared;
using BlazorPlugin2.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorPlugin2.Client;

public partial class App
{
    private List<Package> _packages = [];
    
    [Inject]
    public required IPackageRepository Repo { get; set; }
    
    [Inject]
    public required IInterop DOMInterop { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _packages = await Repo.GetList();
        
        foreach (var package in _packages)
        {
            await Repo.Load(package);
            
            foreach (var asset in package.Assets)
            {
                await LoadAsset(package, asset);
            }
        }
    }

    private async Task LoadAsset(Package package, Asset asset)
    { 
        var id = $"{package.Name}{asset.Path[..asset.Path.LastIndexOf('.')]}";

        switch (asset.Type)
        {
            case "css":
                await DOMInterop.IncludeLink(id, $"/_content/{package.Name}/{asset.Path}");
                break;
            case "js":
                await DOMInterop.IncludeScript(id, $"/_content/{package.Name}/{asset.Path}");
                break;
        }
    }
}