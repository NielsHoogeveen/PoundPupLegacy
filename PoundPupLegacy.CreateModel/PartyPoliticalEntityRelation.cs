namespace PoundPupLegacy.CreateModel;

public abstract record PartyPoliticalEntityRelation: Node
{
    private PartyPoliticalEntityRelation() { }
    public required PartyPoliticalEntityRelationDetails PartyPoliticalEntityRelationDetails { get; init; }
    public sealed record ToCreateForExistingParty: PartyPoliticalEntityRelation, NodeToCreate
    {
        public required int PartyId { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
    }
    public sealed record ToCreateForNewParty: PartyPoliticalEntityRelation, NodeToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public ToCreateForExistingParty ResolveParty(int partyId)
        {
            return new ToCreateForExistingParty {
                PartyId = partyId,
                NodeDetails = NodeDetails,
                Identification = Identification,
                PartyPoliticalEntityRelationDetails = PartyPoliticalEntityRelationDetails
            };
        }
    }
    public sealed record ToUpdate: PartyPoliticalEntityRelation, NodeToUpdate
    {
        public required int PartyId { get; init; }
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}

public sealed record PartyPoliticalEntityRelationDetails
{
    public required int PoliticalEntityId { get; init; }
    public required int PartyPoliticalEntityRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
}