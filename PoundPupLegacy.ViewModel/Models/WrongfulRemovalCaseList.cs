namespace PoundPupLegacy.ViewModel.Models;
[JsonSerializable(typeof(WrongfulRemovalCaseList))]
public partial class WrongfulRemovalCaseListJsonContext : JsonSerializerContext { }

public sealed record WrongfulRemovalCaseList : PagedListBase<CaseListEntry>
{
}
