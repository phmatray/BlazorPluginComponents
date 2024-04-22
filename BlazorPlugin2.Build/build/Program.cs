using System;
using System.Threading.Tasks;
using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
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
    public string MsBuildConfiguration { get; }
    
    public string SolutionPath { get; }
    
    public string OutputDirectory { get; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        MsBuildConfiguration = context.Argument("configuration", "Release");
        SolutionPath = "../../BlazorPlugin2.sln";
        OutputDirectory = "../../Server/";
    }
}

[TaskName("Clean")]
public sealed class CleanTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Cleaning output directories...");
        context.CleanDirectory($"../../RazorClassLibrary1/bin/{context.MsBuildConfiguration}");
        context.CleanDirectory($"../../RazorClassLibrary2/bin/{context.MsBuildConfiguration}");
        context.CleanDirectory($"../../RazorClassLibrary3/bin/{context.MsBuildConfiguration}");
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
            $"../../RazorClassLibrary2/bin/{context.MsBuildConfiguration}/net8.0/RazorClassLibrary2.dll",
            $"../../RazorClassLibrary2/obj/{context.MsBuildConfiguration}/net8.0/scopedcss/bundle/RazorClassLibrary2.styles.css",
            "../../RazorClassLibrary2/wwwroot/background.png"
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

[TaskName("Default")]
[IsDependentOn(typeof(CopyFilesTask))]
public sealed class DefaultTask : FrostingTask
{
}