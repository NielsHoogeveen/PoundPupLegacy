namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CaseTypeListEntry))]
public partial class CaseTypeListEntryJsonContext : JsonSerializerContext { }

public sealed record CaseTypeListEntry : ListEntryBase
{
    public required string Text { get; init; }
}
