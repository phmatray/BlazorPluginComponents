using BlazorPlugin2.Server.Endpoints.NuGetPackages;

namespace BlazorPlugin2.Server.Endpoints;

public static class EndpointsMapper
{
    public static void MapEndpoints(
        this WebApplication app)
    {
        app.MapPackageListEndpoint();
        app.MapPackageCreateEndpoint();
    }
}