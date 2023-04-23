namespace PoundPupLegacy.ViewModel.Models;

public record SubdivisionListEntry: ListEntry
{
    public required string Title { get; init; }

    public required string Path { get; init; }
}
