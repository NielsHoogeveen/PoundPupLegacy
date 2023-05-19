namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(WrongfulRemovalCase))]
public partial class WrongfulRemovalCaseJsonContext : JsonSerializerContext { }

public sealed record WrongfulRemovalCase : CaseBase
{
}
