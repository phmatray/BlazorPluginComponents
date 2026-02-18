using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace RazorClassLibrary3;

public partial class Component3Bis
{
    [Inject]
    public required IJSRuntime JS { get; set; }
    
    private void JsCall()
    {
        JS.InvokeVoidAsync("showPrompt", "hello");
    }
}