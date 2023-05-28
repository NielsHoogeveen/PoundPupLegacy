namespace PoundPupLegacy.CreateModel;

public abstract record BindingCountry : TopLevelCountry
{
    private BindingCountry() { }
    public required TopLevelCountryDetails TopLevelCountryDetails { get; init; }
    public required CountryDetails CountryDetails { get; init; }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<BindingCountryToCreate, T> create, Func<BindingCountryToUpdate, T> update);
    public abstract void Match(Action<BindingCountryToCreate> create, Action<BindingCountryToUpdate> update);

    public sealed record BindingCountryToCreate : BindingCountry, TopLevelCountryToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<BindingCountryToCreate, T> create, Func<BindingCountryToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<BindingCountryToCreate> create, Action<BindingCountryToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record BindingCountryToUpdate : BindingCountry, TopLevelCountryToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<BindingCountryToCreate, T> create, Func<BindingCountryToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<BindingCountryToCreate> create, Action<BindingCountryToUpdate> update)
        {
            update(this);
        }
    }
}
