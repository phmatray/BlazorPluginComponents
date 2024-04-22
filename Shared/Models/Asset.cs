namespace BlazorPlugin2.Shared;

/// <summary>
/// Represents an asset within a package, providing details about its type and file path.
/// </summary>
/// <param name="Type">The type of the asset (e.g., "js" for JavaScript, "css" for Cascading Style Sheets).</param>
/// <param name="Path">The file path of the asset relative to the package it belongs to.</param>
public record Asset(string Type, string Path);