namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(NonSpecificCaseListEntry))]
public partial class NonSpecificCaseListEntryJsonContext : JsonSerializerContext { }

public sealed record NonSpecificCaseListEntry : CaseListEntry
{
    public required string CaseType { get; init; }
}
