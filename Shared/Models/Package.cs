using System.Reflection;
using System.Xml;

namespace BlazorPlugin2.Shared;

/// <summary>
/// Represents a package that can be dynamically loaded into the application, including its assets and components.
/// </summary>
public class Package
{
    public static readonly string[] ValidFormats =
    [
        ".dll", ".pdb",
        ".css", ".js",
        ".png", ".jpg", ".jpeg", ".gif",
        ".json", ".txt", ".csv"
    ];

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
    /// The list of assets included in the package.
    /// </value>
    public List<Asset> Assets { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the assembly associated with the package, loaded into the application domain.
    /// </summary>
    /// <value>
    /// The main assembly of the package, if loaded.
    /// </value>
    public Assembly? Assembly { get; private set; }
    
    /// <summary>
    /// Gets or sets the symbols assembly for the package, which may include debugging symbols.
    /// </summary>
    /// <value>
    /// The assembly containing debugging symbols for the package, if loaded.
    /// </value>
    public Assembly? Symbols { get; private set; }

    public void LoadAssembly(Assembly assembly)
    {
        Assembly = assembly;
        
        Components = assembly
            .GetExportedTypes()
            .Select(s => (s.FullName ?? "", s.BaseType?.Name ?? ""))
            .ToList();
        
        IsLoaded = true;
    }
    
    public void LoadSymbols(Assembly symbols)
    {
        Symbols = symbols;
    }
    
    /// <summary>
    /// Parses XML data to extract asset details and add them to the list.
    /// </summary>
    /// <param name="assetsXml">The XML document containing asset data.</param>
    public void ParseAssetDetailsFromXml(XmlDocument assetsXml)
    {
        foreach (XmlNode asset in assetsXml.GetElementsByTagName("StaticWebAsset"))
        {
            var content = asset.SelectSingleNode("RelativePath")?.InnerText;

            if (content is null)
            {
                continue;
            }
            
            if (content.EndsWith(".js"))
            {
                AddAsset("js", content);
            }
            else if (content.EndsWith(".css"))
            {
                AddAsset("css", content);
            }
        }
    }
    
    /// <summary>
    /// Adds an asset to the list.
    /// </summary>
    /// <param name="type">The type of the asset (e.g., "js" or "css").</param>
    /// <param name="path">The path to the asset file.</param>
    private void AddAsset(string type, string path)
    {
        Assets.Add(new Asset(type, path));
    }
}