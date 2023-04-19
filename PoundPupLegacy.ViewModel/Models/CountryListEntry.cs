namespace PoundPupLegacy.ViewModel.Models;

public record CountryListEntry : ListEntry
{
    public required string Title { get; init; }
    public required string Path { get; init; }
}
