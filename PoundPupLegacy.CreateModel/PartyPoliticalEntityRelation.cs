namespace PoundPupLegacy.CreateModel;

public abstract record PartyPoliticalEntityRelation: Node
{
    private PartyPoliticalEntityRelation() { }
    public required PartyPoliticalEntityRelationDetails PartyPoliticalEntityRelationDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToCreateForExistingParty: PartyPoliticalEntityRelation, NodeToCreate
    {
        public required int PartyId { get; init; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
    }
    public sealed record ToCreateForNewParty: PartyPoliticalEntityRelation, NodeToCreate
    {
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public ToCreateForExistingParty ResolveParty(int partyId)
        {
            return new ToCreateForExistingParty {
                PartyId = partyId,
                NodeDetailsForCreate = NodeDetailsForCreate,
                IdentificationForCreate = IdentificationForCreate,
                PartyPoliticalEntityRelationDetails = PartyPoliticalEntityRelationDetails
            };
        }
    }
    public sealed record ToUpdate: PartyPoliticalEntityRelation, NodeToUpdate
    {
        public required int PartyId { get; init; }
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
    }
}

public sealed record PartyPoliticalEntityRelationDetails
{
    public required int PoliticalEntityId { get; init; }
    public required int PartyPoliticalEntityRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
}