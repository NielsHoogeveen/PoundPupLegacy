namespace PoundPupLegacy.CreateModel;

public abstract record UnitedStatesPoliticalPartyAffiliation: Nameable
{
    private UnitedStatesPoliticalPartyAffiliation() { }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public required UnitedStatesPoliticalPartyAffliationDetails UnitedStatesPoliticalPartyAffliationDetails { get; init; }
    public abstract T Match<T>(Func<UnitedStatesPoliticalPartyAffiliationToCreate, T> create, Func<UnitedStatesPoliticalPartyAffiliationToUpdate, T> update);
    public abstract void Match(Action<UnitedStatesPoliticalPartyAffiliationToCreate> create, Action<UnitedStatesPoliticalPartyAffiliationToUpdate> update);
    public sealed record UnitedStatesPoliticalPartyAffiliationToCreate : UnitedStatesPoliticalPartyAffiliation, NameableToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
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
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
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