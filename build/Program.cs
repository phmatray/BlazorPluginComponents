using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.Pack;
using Cake.Common.Tools.GitVersion;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace Build;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}


public sealed class BuildContext : FrostingContext
{
    public BuildContext(ICakeContext context)
        : base(context)
    {
        MsBuildConfiguration = context.Argument("configuration", "Release");
    }

    public string MsBuildConfiguration { get; }
    
    public string SolutionPath
        => "../BlazorPlugin2.sln";
    
    public string OutputDirectory
        => "../src/Server/";
    
    public string ArtifactsDirectory
        => "../nupkgs";

    public DotNetPackSettings PackSettings
        => new()
        {
            Configuration = MsBuildConfiguration,
            OutputDirectory = ArtifactsDirectory,
            IncludeSymbols = true,
        };

    public string Version { get; set; } = "0.0.0";
}


[TaskName("Clean")]
public sealed class CleanTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Cleaning output directories...");

        // clean using globbing patterns
        context.CleanDirectories($"../**/bin/{context.MsBuildConfiguration}");
    }
}


[TaskName("Restore")]
[TaskDescription("Restoring the solution dependencies")]
[IsDependentOn(typeof(CleanTask))]
public sealed class RestoreTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        // Fetching all csproj files within the solution directory
        var projects = context.GetFiles("../src/**/*.csproj");

        // Iterating through each project and restoring its dependencies
        foreach (var project in projects)
        {
            context.Log.Information($"Restoring {project.GetFilename()}");
            context.DotNetRestore(project.FullPath);
        }
    }
}


[TaskName("Version")]
public sealed class VersionTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var gitVersionSettings = new GitVersionSettings
        {
            UpdateAssemblyInfo = true,
        };
        
        // Running GitVersion to update assembly info and fetch version
        GitVersion result = context.GitVersion(gitVersionSettings);
        
        // Storing the NuGet version from GitVersion result
        context.Version = result.NuGetVersionV2;
        
        // Logging the version to the output
        context.Log.Information($"Version: {context.Version}");
    }
}

[TaskName("Build")]
[IsDependentOn(typeof(CleanTask))]
public sealed class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Building the solution...");
        context.DotNetBuild(context.SolutionPath, new DotNetBuildSettings
        {
            Configuration = context.MsBuildConfiguration,
            NoRestore = true // Assumes restore is done separately if needed
        });
    }
}

[TaskName("CopyFiles")]
[IsDependentOn(typeof(BuildTask))]
public sealed class CopyFilesTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        // Define the source files to copy
        var sourceFiles = new[]
        {
            $"../src/RazorClassLibrary2/bin/{context.MsBuildConfiguration}/net8.0/RazorClassLibrary2.dll",
            $"../src/RazorClassLibrary2/obj/{context.MsBuildConfiguration}/net8.0/scopedcss/bundle/RazorClassLibrary2.styles.css",
            "../src/RazorClassLibrary2/wwwroot/background.png"
        };
        
        // Ensure the output directory exists
        context.EnsureDirectoryExists($"{context.OutputDirectory}wwwroot/RazorClassLibrary2");

        // Copy the files to the output directory
        foreach (var sourceFile in sourceFiles)
        {
            context.Log.Information($"Copying {sourceFile} to output directory...");
            context.CopyFileToDirectory(sourceFile, $"{context.OutputDirectory}wwwroot/RazorClassLibrary2");
        }
    }
}

// Create NuGet packages for the Razor class libraries
[TaskName("Pack")]
[IsDependentOn(typeof(BuildTask))]
public sealed class PackTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Packing Razor class libraries...");
        context.DotNetPack("../src/RazorClassLibrary1/RazorClassLibrary1.csproj", context.PackSettings);
        context.DotNetPack("../src/RazorClassLibrary2/RazorClassLibrary2.csproj", context.PackSettings);
        context.DotNetPack("../src/RazorClassLibrary3/RazorClassLibrary3.csproj", context.PackSettings);
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(CopyFilesTask))]
[IsDependentOn(typeof(PackTask))]
public sealed class DefaultTask : FrostingTask;