namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(NonSpecificCaseListEntry))]
public partial class NonSpecificCaseListEntryJsonContext : JsonSerializerContext { }

public sealed record NonSpecificCaseListEntry : CaseTeaserListEntry
{
    public required string CaseType { get; init; }
}
