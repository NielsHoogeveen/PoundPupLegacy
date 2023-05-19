namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(WrongfulRemovalCases))]
public partial class WrongfulRemovalCasesJsonContext : JsonSerializerContext { }

public sealed record WrongfulRemovalCases : TermedListBase<WrongfulRemovalCaseList, CaseListEntry>
{
}
