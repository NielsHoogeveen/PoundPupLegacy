namespace PoundPupLegacy.ViewModel.Models;

public record CountryListEntry
{
    public required string Name { get; init; }
    public required string Path { get; init; }
}
