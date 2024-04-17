using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorPlugin2.Client.Pages;

public partial class Index
{
    private Type? _componentType;
    private Type? _componentType2;
    private Assembly? _assembly;
    private List<IBrowserFile> _files = [];
    private List<Type>? _types;
    
    [Inject]
    public required HttpClient Http { get; set; }
    
    [Inject]
    public required Interop DOMInterop { get; set; }
    
    [Inject]
    public required NavigationManager MyNavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            const string componentPackage = "RazorClassLibrary2";
            const string component = "Component2";
            var stream = await Http.GetStreamAsync($"{MyNavigationManager.BaseUri}/{componentPackage}/{componentPackage}.dll");
            var assembly = AssemblyLoadContext.Default.LoadFromStream(stream);
            _componentType = assembly.GetType(componentPackage + "." + component);
            await DOMInterop.IncludeLink(componentPackage, $"/{componentPackage}/{componentPackage}.styles.css");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void LoadFiles(InputFileChangeEventArgs e)
    {
        _files = e.GetMultipleFiles().ToList();
    }

    private async void ConfirmUpdate()
    {
        try
        {
            foreach (var file in _files)
            {
                if (file.Name.EndsWith(".dll"))
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream().CopyToAsync(ms);
                    var bytes = ms.ToArray();
                    _assembly = Assembly.Load(bytes);
                    var assemblyName = _assembly.GetName().Name ?? "";
                    _types = _assembly.GetExportedTypes().ToList();
                }
                else if (file.Name.EndsWith(".css"))
                {
                    using StreamReader ms = new(file.OpenReadStream());
                    var css = await ms.ReadToEndAsync();
                    await DOMInterop.AddLink(file.Name, css);
                }
            }

            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}