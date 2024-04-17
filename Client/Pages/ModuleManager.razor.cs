using BlazorPlugin2.Shared;
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
    public required Interop DOMInterop { get; set; }
    
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
            var id = $"{package.Name}{asset.Item2[..asset.Item2.LastIndexOf('.')]}";

            switch (asset.Item1)
            {
                case "css":
                    DOMInterop.IncludeLink(id, $"/_content/{package.Name}/{asset.Item2}");
                    break;
                case "js":
                    DOMInterop.IncludeScript(id, $"/_content/{package.Name}/{asset.Item2}");
                    break;
            }
        }
    }
}