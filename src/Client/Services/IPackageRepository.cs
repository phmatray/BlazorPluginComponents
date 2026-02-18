using BlazorPlugin2.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorPlugin2.Client.Services;

/// <summary>
/// Provides an interface to manage package repositories, including loading, uploading, and checking package status.
/// </summary>
public interface IPackageRepository
{
    /// <summary>
    /// Checks if a package is already loaded.
    /// </summary>
    /// <param name="packageName">The name of the package to check.</param>
    /// <returns>True if the package is loaded; otherwise, false.</returns>
    bool CheckLoaded(string packageName);

    /// <summary>
    /// Retrieves a list of all packages.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of packages.</returns>
    Task<List<Package>> GetList();

    /// <summary>
    /// Loads a package into the application.
    /// </summary>
    /// <param name="package">The package to load.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is true if the package was successfully loaded; otherwise, false.</returns>
    Task<bool> Load(Package package);

    /// <summary>
    /// Uploads a new package to the repository.
    /// </summary>
    /// <param name="nugetPackage">The package file to upload.</param>
    /// <returns>A task representing the asynchronous operation of the upload.</returns>
    Task Upload(IBrowserFile nugetPackage);
}