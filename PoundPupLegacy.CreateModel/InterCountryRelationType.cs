namespace PoundPupLegacy.CreateModel;

public abstract record InterCountryRelationType : EndoRelationType
{
    private InterCountryRelationType() { }
    public required EndoRelationTypeDetails EndoRelationTypeDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<InterCountryRelationTypeToCreate, T> create, Func<InterCountryRelationTypeToUpdate, T> update);
    public abstract void Match(Action<InterCountryRelationTypeToCreate> create, Action<InterCountryRelationTypeToUpdate> update);

    public sealed record InterCountryRelationTypeToCreate : InterCountryRelationType, EndoRelationTypeToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<InterCountryRelationTypeToCreate, T> create, Func<InterCountryRelationTypeToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<InterCountryRelationTypeToCreate> create, Action<InterCountryRelationTypeToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record InterCountryRelationTypeToUpdate : InterCountryRelationType, EndoRelationTypeToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<InterCountryRelationTypeToCreate, T> create, Func<InterCountryRelationTypeToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<InterCountryRelationTypeToCreate> create, Action<InterCountryRelationTypeToUpdate> update)
        {
            update(this);
        }
    }
}
