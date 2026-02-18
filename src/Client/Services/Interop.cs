using Microsoft.JSInterop;

namespace BlazorPlugin2.Client.Services;

/// <summary>
/// Provides JavaScript interop functionalities for a Blazor application.
/// </summary>
/// <param name="jsRuntime">The JSRuntime instance used for JS interop calls.</param>
public class Interop(IJSRuntime jsRuntime) : IInterop
{
    /// <inheritdoc/>
    public Task IncludeLink(string id, string href)
    {
        try
        {
            jsRuntime.InvokeVoidAsync("BlazorPlugin2.Interop.includeLink", id, href);
            return Task.CompletedTask;
        }
        catch
        {
            return Task.CompletedTask;
        }
    }

    /// <inheritdoc/>
    public Task AddLink(string id, string style, string place = "head")
    {
        try
        {
            jsRuntime.InvokeVoidAsync("BlazorPlugin2.Interop.addLink", id, style, place);
            return Task.CompletedTask;
        }
        catch
        {
            return Task.CompletedTask;
        }
    }

    /// <inheritdoc/>
    public Task IncludeScript(string id, string src)
    {
        try
        {
            jsRuntime.InvokeVoidAsync("BlazorPlugin2.Interop.includeScript", id, src);
            return Task.CompletedTask;
        }
        catch
        {
            return Task.CompletedTask;
        }
    }
}
