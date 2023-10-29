namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(ChildTraffickingCases))]
public partial class ChildTraffickingCasesJsonContext : JsonSerializerContext { }

public sealed record ChildTraffickingCases : TermedListBase<ChildTraffickingCaseList, CaseTeaserListEntry>
{
}
