namespace PoundPupLegacy.CreateModel;

public sealed record NewRepresentativeHouseBillAction : NewNodeBase, EventuallyIdentifiableRepresentativeHouseBillAction
{
    public required int RepresentativeId { get; init; }
    public required int HouseBillId { get; init; }
    public required int BillActionTypeId { get; init; }
    public required DateTime Date { get; init; }
}
public sealed record ExistingRepresentativeHouseBillAction : ExistingNodeBase, ImmediatelyIdentifiableRepresentativeHouseBillAction
{
    public required int RepresentativeId { get; init; }
    public required int HouseBillId { get; init; }
    public required int BillActionTypeId { get; init; }
    public required DateTime Date { get; init; }
}
public interface ImmediatelyIdentifiableRepresentativeHouseBillAction : RepresentativeHouseBillAction, ImmediatelyIdentifiableNode
{
}
public interface EventuallyIdentifiableRepresentativeHouseBillAction : RepresentativeHouseBillAction, EventuallyIdentifiableNode
{
}
public interface RepresentativeHouseBillAction : Node
{
    int RepresentativeId { get; }
    int HouseBillId { get; }
    int BillActionTypeId { get; }
    DateTime Date { get; }
}
