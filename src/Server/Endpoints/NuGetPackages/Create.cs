using System.IO.Compression;

namespace BlazorPlugin2.Server.Endpoints.NuGetPackages;

public static class Create
{
    public static void MapPackageCreateEndpoint(
        this WebApplication app)
    {
        app.MapPost("/module-manager", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(
        IWebHostEnvironment env,
        IFormFile file)
    {
        // Check folder name
        using MemoryStream ms = new();
        await file.OpenReadStream().CopyToAsync(ms);
        var bytes = ms.ToArray();
        var folderName = file.FileName.Substring(0, file.FileName.IndexOf('.'));
    
        // Create server path
        string path = Path.Combine(env.WebRootPath, "_content", folderName);
        Directory.CreateDirectory(path);
    
        // Save resources
        await LoadNuget(bytes, path);
    
        return Results.Created($"/_content/{folderName}", folderName);
    }
    
    private static async Task LoadNuget(byte[] nugetFile, string folder)
    {
        using var archive = new ZipArchive(new MemoryStream(nugetFile));

        // Read all 
        foreach (var entry in archive.Entries)
        {
            if (Package.ValidFormats.Contains(Path.GetExtension(entry.Name))
                || entry.Name == "Microsoft.AspNetCore.StaticWebAssets.props") // Static content specification
            {
                string path = Path.Combine(folder, entry.Name);
                await using Stream zipStream = entry.Open();
                await using FileStream fileStream = new FileStream(path, FileMode.Create);
                await zipStream.CopyToAsync(fileStream);
            }
        }
    }
}
