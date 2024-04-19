using System.Reflection;

namespace BlazorPlugin2.Shared;

/// <summary>
/// Represents a package that can be dynamically loaded into the application, including its assets and components.
/// </summary>
public class Package
{
    /// <summary>
    /// Gets or sets the name of the package.
    /// </summary>
    /// <value>
    /// The name of the package.
    /// </value>
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the version of the package.
    /// </summary>
    /// <value>
    /// The version string of the package.
    /// </value>
    public string? Version { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the package is currently loaded.
    /// </summary>
    /// <value>
    /// <c>true</c> if the package is loaded; otherwise, <c>false</c>.
    /// </value>
    public bool IsLoaded { get; set; }
    
    /// <summary>
    /// Gets or sets the list of components available in the package.
    /// </summary>
    /// <value>
    /// A list of tuples where each tuple contains the full name and the base type name of a component.
    /// </value>
    public List<(string, string)> Components { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the list of assets included in the package, such as scripts and stylesheets.
    /// </summary>
    /// <value>
    /// A list of tuples where each tuple contains the asset type (e.g., "js" or "css") and the path to the asset.
    /// </value>
    public List<(string, string)> Assets { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the assembly associated with the package, loaded into the application domain.
    /// </summary>
    /// <value>
    /// The main assembly of the package, if loaded.
    /// </value>
    public Assembly? Assembly { get; set; }
    
    /// <summary>
    /// Gets or sets the symbols assembly for the package, which may include debugging symbols.
    /// </summary>
    /// <value>
    /// The assembly containing debugging symbols for the package, if loaded.
    /// </value>
    public Assembly? Symbols { get; set; }
}
