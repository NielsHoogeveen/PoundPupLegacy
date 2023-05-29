namespace PoundPupLegacy.CreateModel;

public abstract record BasicOrganization : Organization
{
    private BasicOrganization() { }
    
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract OrganizationDetails OrganizationDetails { get; }
    public abstract T Match<T>(Func<BasicOrganizationToCreate, T> create, Func<BasicOrganizationToUpdate, T> update);
    public abstract void Match(Action<BasicOrganizationToCreate> create, Action<BasicOrganizationToUpdate> update);

    public sealed record BasicOrganizationToCreate : BasicOrganization, OrganizationToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public override OrganizationDetails OrganizationDetails => OrganizationDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetailsForCreate { get; init; }
        public required OrganizationDetails.OrganizationDetailsForCreate OrganizationDetailsForCreate { get; init; }
        public override T Match<T>(Func<BasicOrganizationToCreate, T> create, Func<BasicOrganizationToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<BasicOrganizationToCreate> create, Action<BasicOrganizationToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record BasicOrganizationToUpdate : BasicOrganization, OrganizationToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public override OrganizationDetails OrganizationDetails => OrganizationDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetailsForUpdate { get; init; }
        public required OrganizationDetails.OrganizationDetailsForUpdate OrganizationDetailsForUpdate { get; init; }   
        public override T Match<T>(Func<BasicOrganizationToCreate, T> create, Func<BasicOrganizationToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<BasicOrganizationToCreate> create, Action<BasicOrganizationToUpdate> update)
        {
            update(this);
        }
    }
}
