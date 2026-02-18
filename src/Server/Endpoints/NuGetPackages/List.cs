namespace BlazorPlugin2.Server.Endpoints.NuGetPackages;

public static class List
{
    public static void MapPackageListEndpoint(
        this WebApplication app)
    {
        app.MapGet("/module-manager", HandleAsync);
    }
    
    private static Task<CollectionResponse<Package>> HandleAsync(
        IWebHostEnvironment env)
    {
        string path = Path.Combine(env.WebRootPath, "_content");

        List<Package> packages = [];
            
        foreach (var folder in Directory.GetDirectories(path))
        {
            packages.Add(new Package { Name = Path.GetFileName(folder) });
        }
        
        return Task.FromResult(new CollectionResponse<Package>(packages));
    }
}