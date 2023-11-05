namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CountrySubdivisionListEntry))]
[JsonSerializable(typeof(SubdivisionListEntry))]
public partial class SubdivisionListEntryJsonContext : JsonSerializerContext { }

public sealed record SubdivisionListEntry : ListEntryBase
{
    public required CountryListEntry Country { get; init; }
}
public sealed record CountrySubdivisionListEntry : ListEntryBase
{
}
