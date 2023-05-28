namespace PoundPupLegacy.CreateModel;

public abstract record CountryAndIntermediateLevelSubdivision : CountryAndSubdivision, ISOCodedFirstLevelSubdivision, IntermediateLevelSubdivision, CountryAndFirstLevelSubdivision
{
    private CountryAndIntermediateLevelSubdivision() { }
    public required TopLevelCountryDetails TopLevelCountryDetails { get; init; }
    public required CountryDetails CountryDetails { get; init; }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public required ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; init; }
    public required SubdivisionDetails SubdivisionDetails { get; init; }

    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<CountryAndIntermediateLevelSubdivisionToCreate, T> create, Func<CountryAndIntermediateLevelSubdivisionToUpdate, T> update);
    public abstract void Match(Action<CountryAndIntermediateLevelSubdivisionToCreate> create, Action<CountryAndIntermediateLevelSubdivisionToUpdate> update);

    public sealed record CountryAndIntermediateLevelSubdivisionToCreate : CountryAndIntermediateLevelSubdivision, CountryAndSubdivisionToCreate, ISOCodedFirstLevelSubdivisionToCreate, IntermediateLevelSubdivisionToCreate, CountryAndFirstLevelSubdivisionToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<CountryAndIntermediateLevelSubdivisionToCreate, T> create, Func<CountryAndIntermediateLevelSubdivisionToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<CountryAndIntermediateLevelSubdivisionToCreate> create, Action<CountryAndIntermediateLevelSubdivisionToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record CountryAndIntermediateLevelSubdivisionToUpdate : CountryAndIntermediateLevelSubdivision, CountryAndSubdivisionToUpdate, ISOCodedFirstLevelSubdivisionToUpdate, IntermediateLevelSubdivisionToUpdate, CountryAndFirstLevelSubdivisionToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<CountryAndIntermediateLevelSubdivisionToCreate, T> create, Func<CountryAndIntermediateLevelSubdivisionToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<CountryAndIntermediateLevelSubdivisionToCreate> create, Action<CountryAndIntermediateLevelSubdivisionToUpdate> update)
        {
            update(this);
        }
    }
}

