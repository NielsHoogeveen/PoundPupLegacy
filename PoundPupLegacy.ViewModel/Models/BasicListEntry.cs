namespace PoundPupLegacy.ViewModel.Models;

public record BasicListEntry : ListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }
}
