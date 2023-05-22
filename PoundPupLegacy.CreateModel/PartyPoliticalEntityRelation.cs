namespace PoundPupLegacy.CreateModel;

public sealed record NewPartyPoliticalEntityRelation : NewNodeBase, EventuallyIdentifiablePartyPoliticalEntityRelation
{
    public required int PartyId { get; init; }
    public required int PoliticalEntityId { get; init; }
    public required int PartyPoliticalEntityRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
}
public sealed record ExistingPartyPoliticalEntityRelation : ExistingNodeBase, ImmediatelyIdentifiablePartyPoliticalEntityRelation
{
    public required int PartyId { get; init; }
    public required int PoliticalEntityId { get; init; }
    public required int PartyPoliticalEntityRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
}
public interface ImmediatelyIdentifiablePartyPoliticalEntityRelation : PartyPoliticalEntityRelation, ImmediatelyIdentifiableNode
{
}
public interface EventuallyIdentifiablePartyPoliticalEntityRelation : PartyPoliticalEntityRelation, EventuallyIdentifiableNode
{
}
public interface PartyPoliticalEntityRelation : Node
{
    int PartyId { get; }
    int PoliticalEntityId { get; }
    int PartyPoliticalEntityRelationTypeId { get; }
    DateTimeRange DateRange { get; }
    int? DocumentIdProof { get; }
}
