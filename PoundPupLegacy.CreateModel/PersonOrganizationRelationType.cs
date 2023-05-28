namespace PoundPupLegacy.CreateModel;

public abstract record PersonOrganizationRelationType : Nameable
{
    private PersonOrganizationRelationType() { }
    public required PersonOrganizationRelationTypeDetails PersonOrganizationRelationTypeDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<PersonOrganizationRelationTypeToCreate, T> create, Func<PersonOrganizationRelationTypeToUpdate, T> update);
    public abstract void Match(Action<PersonOrganizationRelationTypeToCreate> create, Action<PersonOrganizationRelationTypeToUpdate> update);

    public sealed record PersonOrganizationRelationTypeToCreate : PersonOrganizationRelationType, NameableToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
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
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
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
