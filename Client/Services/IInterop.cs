namespace BlazorPlugin2.Client.Services;

/// <summary>
/// Defines a set of methods for interacting with JavaScript interop in a Blazor application.
/// </summary>
public interface IInterop
{
    /// <summary>
    /// Asynchronously includes a CSS link in the document.
    /// </summary>
    /// <param name="id">The identifier for the link tag.</param>
    /// <param name="href">The URL of the CSS file.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task IncludeLink(string id, string href);

    /// <summary>
    /// Asynchronously adds a CSS link to a specified location in the document.
    /// </summary>
    /// <param name="id">The identifier for the link tag.</param>
    /// <param name="style">The content of the style to be added.</param>
    /// <param name="place">The location to add the link tag, defaults to "head".</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddLink(string id, string style, string place = "head");

    /// <summary>
    /// Asynchronously includes a script file in the document.
    /// </summary>
    /// <param name="id">The identifier for the script tag.</param>
    /// <param name="src">The source URL of the script file.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task IncludeScript(string id, string src);
}