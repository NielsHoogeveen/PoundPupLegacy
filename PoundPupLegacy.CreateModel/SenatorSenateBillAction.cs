namespace PoundPupLegacy.CreateModel;

public sealed record NewSenatorSenateBillAction : NewNodeBase, EventuallyIdentifiableSenatorSenateBillAction
{
    public required int SenatorId { get; init; }
    public required int SenateBillId { get; init; }
    public required int BillActionTypeId { get; init; }
    public required DateTime Date { get; init; }
}
public sealed record ExistingSenatorSenateBillAction : ExistingNodeBase, ImmediatelyIdentifiableSenatorSenateBillAction
{
    public required int SenatorId { get; init; }
    public required int SenateBillId { get; init; }
    public required int BillActionTypeId { get; init; }
    public required DateTime Date { get; init; }
}
public interface ImmediatelyIdentifiableSenatorSenateBillAction : SenatorSenateBillAction, ImmediatelyIdentifiableNode
{
}
public interface EventuallyIdentifiableSenatorSenateBillAction : SenatorSenateBillAction, EventuallyIdentifiableNode
{
}
public interface SenatorSenateBillAction : Node
{
    int SenatorId { get; }
    int SenateBillId { get; }
    int BillActionTypeId { get; }
    DateTime Date { get; }
}
