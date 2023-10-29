namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(RelatedCaseType))]
public partial class RelatedCaseTypeJsonContext : JsonSerializerContext { }

public sealed record RelatedCaseType
{
    public required string CaseTypeName { get; init; }
    public required CaseListEntry[] Cases { get; init; }

}
