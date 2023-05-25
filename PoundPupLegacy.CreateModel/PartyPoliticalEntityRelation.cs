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

    public EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty ResolveParty(int partyId)
    {
        return new NewPartyPoliticalEntityRelationForExistingParty {
            PartyId = partyId,
            AuthoringStatusId = AuthoringStatusId,
            ChangedDateTime = ChangedDateTime,
            CreatedDateTime = CreatedDateTime,
            DateRange = DateRange,
            DocumentIdProof = DocumentIdProof,
            NodeTermIds = NodeTermIds,
            NodeTypeId = NodeTypeId,
            OwnerId = OwnerId,
            PoliticalEntityId = PoliticalEntityId,
            PublisherId = PublisherId,
            TenantNodes = TenantNodes,
            Title = Title,
            Id = Id,
            PartyPoliticalEntityRelationTypeId = PartyPoliticalEntityRelationTypeId
        };
    }
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
    public EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty ResolveParty(int partyId);
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
