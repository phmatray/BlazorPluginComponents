using BlazorPlugin2.Client.Services;
using BlazorPlugin2.Shared;
using BlazorPlugin2.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorPlugin2.Client.Pages;

public partial class ModuleManager
{
    private List<Package> _packages = [];
    private List<Type?> _components = [];
    
    [Inject]
    public required IPackageRepository Repo { get; set; }
    
    [Inject]
    public required HttpClient Http { get; set; }
    
    [Inject]
    public required NavigationManager MyNavigationManager { get; set; }
    
    [Inject]
    public required IInterop DOMInterop { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        _packages = await Repo.GetList();
    }

    private async Task OnInputFileChange(InputFileChangeEventArgs e)
    {
        await Repo.Upload(e.File);
    }

    private async Task Load(Package package)
    {
        await Repo.Load(package);
    }

    private void LoadComponent(ChangeEventArgs changeEventArgs, Package package)
    {
        string component = changeEventArgs.Value?.ToString() ?? "";
        _components.Add(package.Assembly?.GetType(component));

        foreach (var asset in package.Assets)
        {
            var id = $"{package.Name}{asset.Path[..asset.Path.LastIndexOf('.')]}";

            switch (asset.Type)
            {
                case "css":
                    DOMInterop.IncludeLink(id, $"/_content/{package.Name}/{asset.Path}");
                    break;
                case "js":
                    DOMInterop.IncludeScript(id, $"/_content/{package.Name}/{asset.Path}");
                    break;
            }
        }
    }
}