namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CountryListEntry))]
public partial class CountryListEntryJsonContext : JsonSerializerContext { }
public record CountryListEntry : ListEntry
{
    public required string Title { get; init; }
    public required string Path { get; init; }
}
