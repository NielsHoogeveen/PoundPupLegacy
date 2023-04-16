namespace PoundPupLegacy.ViewModel.Models;

public record SearchResultListEntry : ListEntry
{
    public required string Path { get; init; }

    public required string Title { get; init; }

    public required string Teaser { get; init; }

    public required string NodeTypeName { get; init; }

    public required int Status { get; init; }

}
