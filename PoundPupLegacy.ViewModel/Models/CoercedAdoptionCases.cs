namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CoercedAdoptionCases))]
public partial class CoercedAdoptionCasesJsonContext : JsonSerializerContext { }

public sealed record CoercedAdoptionCases : TermedListBase<CoercedAdoptionCaseList, CaseListEntry>
{
}
