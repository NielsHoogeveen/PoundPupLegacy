namespace PoundPupLegacy.CreateModel;

public abstract record UnitedStatesPoliticalPartyAffiliation: Nameable
{
    private UnitedStatesPoliticalPartyAffiliation() { }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public required UnitedStatesPoliticalPartyAffliationDetails UnitedStatesPoliticalPartyAffliationDetails { get; init; }
    public abstract T Match<T>(Func<UnitedStatesPoliticalPartyAffiliationToCreate, T> create, Func<UnitedStatesPoliticalPartyAffiliationToUpdate, T> update);
    public abstract void Match(Action<UnitedStatesPoliticalPartyAffiliationToCreate> create, Action<UnitedStatesPoliticalPartyAffiliationToUpdate> update);
    public sealed record UnitedStatesPoliticalPartyAffiliationToCreate : UnitedStatesPoliticalPartyAffiliation, NameableToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<UnitedStatesPoliticalPartyAffiliationToCreate, T> create, Func<UnitedStatesPoliticalPartyAffiliationToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<UnitedStatesPoliticalPartyAffiliationToCreate> create, Action<UnitedStatesPoliticalPartyAffiliationToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record UnitedStatesPoliticalPartyAffiliationToUpdate : UnitedStatesPoliticalPartyAffiliation, NameableToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<UnitedStatesPoliticalPartyAffiliationToCreate, T> create, Func<UnitedStatesPoliticalPartyAffiliationToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<UnitedStatesPoliticalPartyAffiliationToCreate> create, Action<UnitedStatesPoliticalPartyAffiliationToUpdate> update)
        {
            update(this);
        }
    }
}


public sealed record UnitedStatesPoliticalPartyAffliationDetails
{
    public required int? UnitedStatesPoliticalPartyId { get; init; }
}