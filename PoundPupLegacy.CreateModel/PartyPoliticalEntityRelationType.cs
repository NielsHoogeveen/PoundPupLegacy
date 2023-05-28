namespace PoundPupLegacy.CreateModel;

public abstract record PartyPoliticalEntityRelationType : Nameable
{
    private PartyPoliticalEntityRelationType() { }
    public required PartyPoliticalEntityRelationTypeDetails PartyPoliticalEntityRelationTypeDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<PartyPoliticalEntityRelationTypeToCreate, T> create, Func<PartyPoliticalEntityRelationTypeToUpdate, T> update);
    public abstract void Match(Action<PartyPoliticalEntityRelationTypeToCreate> create, Action<PartyPoliticalEntityRelationTypeToUpdate> update);

    public sealed record PartyPoliticalEntityRelationTypeToCreate : PartyPoliticalEntityRelationType, NameableToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<PartyPoliticalEntityRelationTypeToCreate, T> create, Func<PartyPoliticalEntityRelationTypeToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<PartyPoliticalEntityRelationTypeToCreate> create, Action<PartyPoliticalEntityRelationTypeToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record PartyPoliticalEntityRelationTypeToUpdate : PartyPoliticalEntityRelationType, NameableToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<PartyPoliticalEntityRelationTypeToCreate, T> create, Func<PartyPoliticalEntityRelationTypeToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<PartyPoliticalEntityRelationTypeToCreate> create, Action<PartyPoliticalEntityRelationTypeToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record PartyPoliticalEntityRelationTypeDetails
{
    public required bool HasConcreteSubtype { get; init; }
}
