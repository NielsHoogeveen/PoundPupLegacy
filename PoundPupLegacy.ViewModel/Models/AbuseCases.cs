namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(AbuseCases))]
public partial class AbuseCasesJsonContext : JsonSerializerContext { }

public sealed record AbuseCases : TermedListBase<AbuseCaseList, CaseListEntry>
{
}
