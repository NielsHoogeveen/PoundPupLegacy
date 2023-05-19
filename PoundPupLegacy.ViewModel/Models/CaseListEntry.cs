namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CaseListEntry))]
public partial class CaseListEntryJsonContext : JsonSerializerContext { }

public record CaseListEntry : TaggedTeaserListEntryBase
{
    public required FuzzyDate? Date { get; init; }

}
