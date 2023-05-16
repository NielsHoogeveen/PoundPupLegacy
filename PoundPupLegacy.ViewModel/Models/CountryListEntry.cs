namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CountryListEntry))]
public partial class CountryListEntryJsonContext : JsonSerializerContext { }
public sealed record CountryListEntry : ListEntry
{
    public required string Title { get; init; }
    public required string Path { get; init; }
}
