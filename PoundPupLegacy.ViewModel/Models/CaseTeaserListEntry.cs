namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CaseTeaserListEntry))]
public partial class CaseTeaserListEntryJsonContext : JsonSerializerContext { }

public record CaseTeaserListEntry : TaggedTeaserListEntryBase
{
    public required FuzzyDate? Date { get; init; }

}
