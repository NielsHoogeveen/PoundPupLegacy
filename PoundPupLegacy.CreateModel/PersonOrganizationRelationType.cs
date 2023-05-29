namespace PoundPupLegacy.CreateModel;

public abstract record PersonOrganizationRelationType : Nameable
{
    private PersonOrganizationRelationType() { }
    public required PersonOrganizationRelationTypeDetails PersonOrganizationRelationTypeDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<PersonOrganizationRelationTypeToCreate, T> create, Func<PersonOrganizationRelationTypeToUpdate, T> update);
    public abstract void Match(Action<PersonOrganizationRelationTypeToCreate> create, Action<PersonOrganizationRelationTypeToUpdate> update);

    public sealed record PersonOrganizationRelationTypeToCreate : PersonOrganizationRelationType, NameableToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<PersonOrganizationRelationTypeToCreate, T> create, Func<PersonOrganizationRelationTypeToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<PersonOrganizationRelationTypeToCreate> create, Action<PersonOrganizationRelationTypeToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record PersonOrganizationRelationTypeToUpdate : PersonOrganizationRelationType, NameableToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<PersonOrganizationRelationTypeToCreate, T> create, Func<PersonOrganizationRelationTypeToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<PersonOrganizationRelationTypeToCreate> create, Action<PersonOrganizationRelationTypeToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record PersonOrganizationRelationTypeDetails
{
    public required bool HasConcreteSubtype { get; init; }
}
