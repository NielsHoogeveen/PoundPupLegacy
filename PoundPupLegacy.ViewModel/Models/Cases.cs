namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Cases))]
public partial class CasesJsonContext : JsonSerializerContext { }

public sealed record Cases : PagedListBase<NonSpecificCaseListEntry>
{
    public required CaseTypeListEntry[] CaseTypes { get; init; }

}
