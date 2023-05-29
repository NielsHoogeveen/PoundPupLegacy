namespace PoundPupLegacy.CreateModel;

public abstract record OrganizationType : Nameable
{
    private OrganizationType() { }
    public required OrganizationTypeDetails OrganizationTypeDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<OrganizationTypeToCreate, T> create, Func<OrganizationTypeToUpdate, T> update);
    public abstract void Match(Action<OrganizationTypeToCreate> create, Action<OrganizationTypeToUpdate> update);

    public sealed record OrganizationTypeToCreate : OrganizationType, NameableToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<OrganizationTypeToCreate, T> create, Func<OrganizationTypeToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<OrganizationTypeToCreate> create, Action<OrganizationTypeToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record OrganizationTypeToUpdate : OrganizationType, NameableToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<OrganizationTypeToCreate, T> create, Func<OrganizationTypeToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<OrganizationTypeToCreate> create, Action<OrganizationTypeToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record OrganizationTypeDetails
{
    public required bool HasConcreteSubtype { get; init; }
}
