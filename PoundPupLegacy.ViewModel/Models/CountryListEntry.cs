namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CountryListEntry))]
public partial class CountryListEntryJsonContext : JsonSerializerContext { }
public sealed record CountryListEntry : ListEntryBase
{
}
