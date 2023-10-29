namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(DeportationCases))]
public partial class DeportationCasesJsonContext : JsonSerializerContext { }

public sealed record DeportationCases : TermedListBase<DeportationCaseList, CaseTeaserListEntry>
{
}
