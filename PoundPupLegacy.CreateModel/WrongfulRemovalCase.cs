namespace PoundPupLegacy.CreateModel;

public sealed record NewWrongfulRemovalCase : NewCaseBase, EventuallyIdentifiableWrongfulRemovalCase
{
}
public sealed record ExistingWrongfulRemovalCase : ExistingCaseBase, ImmediatelyIdentifiableWrongfulRemovalCase
{
}
public interface ImmediatelyIdentifiableWrongfulRemovalCase : WrongfulRemovalCase, ImmediatelyIdentifiableCase
{
}
public interface EventuallyIdentifiableWrongfulRemovalCase : WrongfulRemovalCase, EventuallyIdentifiableCase
{
}

public interface WrongfulRemovalCase : Case
{
}
