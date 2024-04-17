using Microsoft.AspNetCore.Components;

namespace BlazorPlugin2.Client.Shared;

public partial class SurveyPrompt
{
    [Parameter]
    public string? Title { get; set; }
}