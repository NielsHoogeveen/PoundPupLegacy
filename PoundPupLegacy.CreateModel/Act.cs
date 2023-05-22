namespace PoundPupLegacy.CreateModel;

public sealed record NewAct : NewNameableBase, EventuallyIdentifiableAct
{
    public DateTime? EnactmentDate { get; init; }
}
public sealed record ExistingAct : ExistingNameableBase, ImmediatelyIdentifiableAct
{
    public DateTime? EnactmentDate { get; init; }
}
public interface ImmediatelyIdentifiableAct : Act, ImmediatelyIdentifiableNameable, ImmediatelyIdentifiableDocumentable
{
}

public interface EventuallyIdentifiableAct: Act, EventuallyIdentifiableNameable, EventuallyIdentifiableDocumentable
{
}

public interface Act
{
    DateTime? EnactmentDate {get;}
}
