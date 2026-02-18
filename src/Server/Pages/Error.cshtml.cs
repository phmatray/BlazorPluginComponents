using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace BlazorPlugin2.Server.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel(ILogger<ErrorModel> logger)
    : PageModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId
        => !string.IsNullOrEmpty(RequestId);

    public void OnGet()
    {
        logger.LogError("An error occurred while processing your request");
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}