namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingWrongfulRemovalCase))]
public partial class ExistingWrongfulRemovalCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewWrongfulRemovalCase))]
public partial class NewWrongfulRemovalCaseJsonContext : JsonSerializerContext { }

public interface WrongfulRemovalCase : Case, ResolvedNode
{
}
public sealed record ExistingWrongfulRemovalCase : ExistingCaseBase, ExistingNode, WrongfulRemovalCase
{
}
public abstract record NewWrongfulRemovalCase : NewCaseBase, ResolvedNewNode, WrongfulRemovalCase
{
}
