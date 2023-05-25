namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingWrongfulRemovalCase))]
public partial class ExistingWrongfulRemovalCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewWrongfulRemovalCase))]
public partial class NewWrongfulRemovalCaseJsonContext : JsonSerializerContext { }

public interface WrongfulRemovalCase : Case, ResolvedNode
{
}
public sealed record ExistingWrongfulRemovalCase : WrongfulRemovalCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}
public abstract record NewWrongfulRemovalCase : WrongfulRemovalCaseBase, ResolvedNewNode
{
}
public abstract record WrongfulRemovalCaseBase : CaseBase, WrongfulRemovalCase
{

}
