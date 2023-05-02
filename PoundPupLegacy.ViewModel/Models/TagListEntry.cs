namespace PoundPupLegacy.ViewModel.Models;

public record TagListEntry : ListEntry
{
    public required string Path { get; init; }

    public required string Title { get; init; }

    public required string NodeTypeName { get; init; }
}
