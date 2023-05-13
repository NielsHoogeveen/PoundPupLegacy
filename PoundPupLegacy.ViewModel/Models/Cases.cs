using System.Text.Json.Serialization;

namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Cases))]
public partial class CasesJsonContext : JsonSerializerContext { }

public record Cases : IPagedList<NonSpecificCaseListEntry>
{
    public required CaseTypeListEntry[] CaseTypes { get; init; }
    public required NonSpecificCaseListEntry[] Entries { get; init; }
    public required int NumberOfEntries { get; init; }

}
