namespace PoundPupLegacy.DomainModel;

public abstract record PartyPoliticalEntityRelation : Node
{
    private PartyPoliticalEntityRelation() { }
    public required PartyPoliticalEntityRelationDetails PartyPoliticalEntityRelationDetails { get; init; }
    public abstract record ToCreate : PartyPoliticalEntityRelation, NodeToCreate
    {
        private ToCreate() { }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public sealed record ForExistingParty : ToCreate
        {
            public required int PartyId { get; init; }
        }
        public sealed record ForNewParty : ToCreate
        {
            public ForExistingParty ResolveParty(int partyId)
            {
                return new ForExistingParty {
                    PartyId = partyId,
                    NodeDetails = NodeDetails,
                    Identification = Identification,
                    PartyPoliticalEntityRelationDetails = PartyPoliticalEntityRelationDetails
                };
            }
        }
    }
    public sealed record ToUpdate : PartyPoliticalEntityRelation, NodeToUpdate
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