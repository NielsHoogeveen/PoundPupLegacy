﻿namespace PoundPupLegacy.CreateModel;

public abstract record UnitedStatesPoliticalParty : Organization
{
    private UnitedStatesPoliticalParty() { }

    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract OrganizationDetails OrganizationDetails { get; }
    public abstract T Match<T>(Func<UnitedStatesPoliticalPartyToCreate, T> create, Func<UnitedStatesPoliticalPartyToUpdate, T> update);
    public abstract void Match(Action<UnitedStatesPoliticalPartyToCreate> create, Action<UnitedStatesPoliticalPartyToUpdate> update);

    public sealed record UnitedStatesPoliticalPartyToCreate : UnitedStatesPoliticalParty, OrganizationToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public override OrganizationDetails OrganizationDetails => OrganizationDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetailsForCreate { get; init; }
        public required OrganizationDetails.OrganizationDetailsForCreate OrganizationDetailsForCreate { get; init; }
        public override T Match<T>(Func<UnitedStatesPoliticalPartyToCreate, T> create, Func<UnitedStatesPoliticalPartyToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<UnitedStatesPoliticalPartyToCreate> create, Action<UnitedStatesPoliticalPartyToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record UnitedStatesPoliticalPartyToUpdate : UnitedStatesPoliticalParty, OrganizationToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public override OrganizationDetails OrganizationDetails => OrganizationDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetailsForUpdate { get; init; }
        public required OrganizationDetails.OrganizationDetailsForUpdate OrganizationDetailsForUpdate { get; init; }
        public override T Match<T>(Func<UnitedStatesPoliticalPartyToCreate, T> create, Func<UnitedStatesPoliticalPartyToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<UnitedStatesPoliticalPartyToCreate> create, Action<UnitedStatesPoliticalPartyToUpdate> update)
        {
            update(this);
        }
    }
}

