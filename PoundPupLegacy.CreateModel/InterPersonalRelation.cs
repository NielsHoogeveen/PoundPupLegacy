namespace PoundPupLegacy.CreateModel;

public sealed record NewInterPersonalRelation : NewNodeBase, EventuallyIdentifiableInterPersonalRelation
{
    public required int InterPersonalRelationTypeId { get; init; }
    public required int PersonIdFrom { get; init; }
    public required int PersonIdTo { get; init; }
    public required DateTimeRange? DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required string? Description { get; init; }
}
public sealed record ExistingInterPersonalRelation : ExistingNodeBase, ImmediatelyIdentifiableInterPersonalRelation
{
    public required int InterPersonalRelationTypeId { get; init; }
    public required int PersonIdFrom { get; init; }
    public required int PersonIdTo { get; init; }
    public required DateTimeRange? DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required string? Description { get; init; }
}
public interface ImmediatelyIdentifiableInterPersonalRelation : InterPersonalRelation, ImmediatelyIdentifiableNode
{
}
public interface EventuallyIdentifiableInterPersonalRelation : InterPersonalRelation, EventuallyIdentifiableNode
{
}
public interface InterPersonalRelation : Node
{
    int InterPersonalRelationTypeId { get; }
    int PersonIdFrom { get; }
    int PersonIdTo { get; }
    DateTimeRange? DateRange { get; }
    int? DocumentIdProof { get; }
    string? Description { get;}
}
