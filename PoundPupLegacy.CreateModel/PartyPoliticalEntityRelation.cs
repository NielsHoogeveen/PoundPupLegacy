namespace PoundPupLegacy.CreateModel;

public sealed record NewPartyPoliticalEntityRelationForExistingParty : NewNodeBase, EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty
{
    public required int PartyId { get; init; }
    public required int PoliticalEntityId { get; init; }
    public required int PartyPoliticalEntityRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
}
public sealed record NewPartyPoliticalEntityRelationForNewParty : NewNodeBase, EventuallyIdentifiablePartyPoliticalEntityRelationForNewParty
{
    public required int? PartyId { get; init; }
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
public interface ImmediatelyIdentifiablePartyPoliticalEntityRelation : PartyPoliticalEntityRelationForExitingParty, ImmediatelyIdentifiableNode
{
}
public interface EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty : PartyPoliticalEntityRelationForExitingParty, EventuallyIdentifiableNode
{
}
public interface EventuallyIdentifiablePartyPoliticalEntityRelationForNewParty : PartyPoliticalEntityRelationForNewParty, EventuallyIdentifiableNode
{
}

public interface PartyPoliticalEntityRelationForExitingParty : PartyPoliticalEntityRelation
{
    int PartyId { get; }
}
public interface PartyPoliticalEntityRelationForNewParty : PartyPoliticalEntityRelation
{
    int? PartyId { get; }
}


public interface PartyPoliticalEntityRelation : Node
{
    int PoliticalEntityId { get; }
    int PartyPoliticalEntityRelationTypeId { get; }
    DateTimeRange DateRange { get; }
    int? DocumentIdProof { get; }
}
