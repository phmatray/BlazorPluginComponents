namespace BlazorPlugin2.Shared.DTOs;

/// <summary>
/// Represents a collection of items, providing details about the items and their count.
/// </summary>
/// <param name="Items">The items in the collection.</param>
/// <typeparam name="T">The type of items in the collection.</typeparam>
public record CollectionResponse<T>(List<T> Items)
{
    public List<T> Items { get; private set; } = Items;
    public int Count { get; private set; } = Items.Count;
}