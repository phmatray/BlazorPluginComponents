using System.Reflection;

namespace BlazorPlugin2.Shared;

public class Package
{
    public string? Name { get; set; }
    public string? Version { get; set; }
    public bool IsLoaded { get; set; }
    public List<(string, string)> Components { get; set; } = [];
    public List<(string, string)> Assets { get; set; } = [];
    public Assembly? Assembly { get; set; }
    public Assembly? Symbols { get; set; }
}
