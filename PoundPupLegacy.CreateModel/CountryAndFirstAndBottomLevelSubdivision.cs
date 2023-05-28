namespace PoundPupLegacy.CreateModel;

public abstract record CountryAndFirstAndBottomLevelSubdivision : CountryAndFirstLevelSubdivision, BottomLevelSubdivision
{
    private CountryAndFirstAndBottomLevelSubdivision() { }
    public required TopLevelCountryDetails TopLevelCountryDetails { get; init; }
    public required CountryDetails CountryDetails { get; init; }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public required ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; init; }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<CountryAndFirstAndBottomLevelSubdivisionToCreate, T> create, Func<CountryAndFirstAndBottomLevelSubdivisionToUpdate, T> update);
    public abstract void Match(Action<CountryAndFirstAndBottomLevelSubdivisionToCreate> create, Action<CountryAndFirstAndBottomLevelSubdivisionToUpdate> update);

    public sealed record CountryAndFirstAndBottomLevelSubdivisionToCreate : CountryAndFirstAndBottomLevelSubdivision, CountryAndFirstLevelSubdivisionToCreate, BottomLevelSubdivisionToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<CountryAndFirstAndBottomLevelSubdivisionToCreate, T> create, Func<CountryAndFirstAndBottomLevelSubdivisionToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<CountryAndFirstAndBottomLevelSubdivisionToCreate> create, Action<CountryAndFirstAndBottomLevelSubdivisionToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record CountryAndFirstAndBottomLevelSubdivisionToUpdate : CountryAndFirstAndBottomLevelSubdivision, CountryAndFirstLevelSubdivisionToUpdate, BottomLevelSubdivisionToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<CountryAndFirstAndBottomLevelSubdivisionToCreate, T> create, Func<CountryAndFirstAndBottomLevelSubdivisionToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<CountryAndFirstAndBottomLevelSubdivisionToCreate> create, Action<CountryAndFirstAndBottomLevelSubdivisionToUpdate> update)
        {
            update(this);
        }
    }
}

