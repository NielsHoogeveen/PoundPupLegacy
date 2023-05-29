namespace PoundPupLegacy.CreateModel;

public abstract record PartyPoliticalEntityRelation: Node
{
    private PartyPoliticalEntityRelation() { }
    public required PartyPoliticalEntityRelationDetails PartyPoliticalEntityRelationDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(
        Func<PartyPoliticalEntityRelationToCreateForExistingParty, T> create,
        Func<PartyPoliticalEntityRelationToCreateForNewParty, T> createNewParty,
        Func<PartyPoliticalEntityRelationToUpdate, T> update
     );
    public abstract void Match(
        Action<PartyPoliticalEntityRelationToCreateForExistingParty> create,
        Action<PartyPoliticalEntityRelationToCreateForNewParty> createNewParty,
        Action<PartyPoliticalEntityRelationToUpdate> update
    );

    public sealed record PartyPoliticalEntityRelationToCreateForExistingParty: PartyPoliticalEntityRelation, NodeToCreate
    {
        public required int PartyId { get; init; }
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(
            Func<PartyPoliticalEntityRelationToCreateForExistingParty, T> create,
            Func<PartyPoliticalEntityRelationToCreateForNewParty, T> createNewParty,
            Func<PartyPoliticalEntityRelationToUpdate, T> update
         )
        { 
            return create(this);
        }
        public override void Match(
            Action<PartyPoliticalEntityRelationToCreateForExistingParty> create,
            Action<PartyPoliticalEntityRelationToCreateForNewParty> createNewParty,
            Action<PartyPoliticalEntityRelationToUpdate> update
        )
        { 
            create(this);
        }
    }
    public sealed record PartyPoliticalEntityRelationToCreateForNewParty: PartyPoliticalEntityRelation, NodeToCreate
    {
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(
            Func<PartyPoliticalEntityRelationToCreateForExistingParty, T> create,
            Func<PartyPoliticalEntityRelationToCreateForNewParty, T> createNewParty,
            Func<PartyPoliticalEntityRelationToUpdate, T> update
         )
        {
            return createNewParty(this);
        }
        public override void Match(
            Action<PartyPoliticalEntityRelationToCreateForExistingParty> create,
            Action<PartyPoliticalEntityRelationToCreateForNewParty> createNewParty,
            Action<PartyPoliticalEntityRelationToUpdate> update
        )
        {
            createNewParty(this);
        }

        public PartyPoliticalEntityRelationToCreateForExistingParty ResolveParty(int partyId)
        {
            return new PartyPoliticalEntityRelationToCreateForExistingParty {
                PartyId = partyId,
                NodeDetailsForCreate = NodeDetailsForCreate,
                IdentificationForCreate = IdentificationForCreate,
                PartyPoliticalEntityRelationDetails = PartyPoliticalEntityRelationDetails
            };
        }
    }
    public sealed record PartyPoliticalEntityRelationToUpdate: PartyPoliticalEntityRelation, NodeToUpdate
    {
        public required int PartyId { get; init; }
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(
    Func<PartyPoliticalEntityRelationToCreateForExistingParty, T> create,
    Func<PartyPoliticalEntityRelationToCreateForNewParty, T> createNewParty,
    Func<PartyPoliticalEntityRelationToUpdate, T> update
 )
        {
            return update(this);
        }
        public override void Match(
            Action<PartyPoliticalEntityRelationToCreateForExistingParty> create,
            Action<PartyPoliticalEntityRelationToCreateForNewParty> createNewParty,
            Action<PartyPoliticalEntityRelationToUpdate> update
        )
        {
            update(this);
        }
    }
}

public sealed record PartyPoliticalEntityRelationDetails
{
    public required int PoliticalEntityId { get; init; }
    public required int PartyPoliticalEntityRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
}