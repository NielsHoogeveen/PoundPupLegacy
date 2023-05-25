namespace PoundPupLegacy.CreateModel;

public sealed record NewInterPersonalRelationForExistingParticipants : NewNodeBase, EventuallyIdentifiableInterPersonalRelationForExistingParticipants
{
    public required int PersonIdFrom { get; init; }
    public required int PersonIdTo { get; init; }
    public required int InterPersonalRelationTypeId { get; init; }
    public required DateTimeRange? DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required string? Description { get; init; }
}
public sealed record NewInterPersonalRelationForNewPersonFrom : NewNodeBase, EventuallyIdentifiableInterPersonalRelationForNewPersonFrom
{
    public required int? PersonIdFrom { get; set; }
    public required int PersonIdTo { get; init; }
    public required int InterPersonalRelationTypeId { get; init; }
    public required DateTimeRange? DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required string? Description { get; init; }
}
public sealed record NewInterPersonalRelationForNewPersonTo : NewNodeBase, EventuallyIdentifiableInterPersonalRelationForNewPersonTo
{
    public required int PersonIdFrom { get; init; }
    public required int? PersonIdTo { get; set; }
    public required int InterPersonalRelationTypeId { get; init; }
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
    int PersonIdFrom { get; }
    int PersonIdTo { get; }
}
public interface EventuallyIdentifiableInterPersonalRelationForExistingParticipants: EventuallyIdentifiableInterPersonalRelation
{
    int PersonIdFrom { get; }
    int PersonIdTo { get; }
}
public interface EventuallyIdentifiableInterPersonalRelationForNewPersonFrom : EventuallyIdentifiableInterPersonalRelation
{
    int? PersonIdFrom { get; set; }
    int PersonIdTo { get; }
}
public interface EventuallyIdentifiableInterPersonalRelationForNewPersonTo : EventuallyIdentifiableInterPersonalRelation
{
    int PersonIdFrom { get; }
    int? PersonIdTo { get; set; }
}
public interface EventuallyIdentifiableInterPersonalRelation : InterPersonalRelation, EventuallyIdentifiableNode
{
}
public interface InterPersonalRelation : Node
{
   
    int InterPersonalRelationTypeId { get; }
    DateTimeRange? DateRange { get; }
    int? DocumentIdProof { get; }
    string? Description { get;}
}
