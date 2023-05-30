namespace PoundPupLegacy.CreateModel;

public abstract record UnitedStatesPoliticalParty : Organization
{
    private UnitedStatesPoliticalParty() { }

    public abstract LocatableDetails LocatableDetails { get; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract OrganizationDetails OrganizationDetails { get; }

    public sealed record ToCreate : UnitedStatesPoliticalParty, OrganizationToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public override OrganizationDetails OrganizationDetails => OrganizationDetailsForCreate;
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetailsForCreate { get; init; }
        public required OrganizationDetails.OrganizationDetailsForCreate OrganizationDetailsForCreate { get; init; }
    }
    public sealed record ToUpdate : UnitedStatesPoliticalParty, OrganizationToUpdate
    {
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public override OrganizationDetails OrganizationDetails => OrganizationDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetailsForUpdate { get; init; }
        public required OrganizationDetails.OrganizationDetailsForUpdate OrganizationDetailsForUpdate { get; init; }
    }
}

