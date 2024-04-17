using Microsoft.JSInterop;

namespace BlazorPlugin2.Client;

public class Interop(IJSRuntime jsRuntime)
{
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
