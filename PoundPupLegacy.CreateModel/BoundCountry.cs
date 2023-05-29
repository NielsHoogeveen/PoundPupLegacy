namespace PoundPupLegacy.CreateModel;

public abstract record BoundCountry : Country, ISOCodedSubdivision
{
    private BoundCountry() { }
    public required BoundCountryDetails BoundCountryDetails { get; init; }
    public required ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; init; }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public required CountryDetails CountryDetails { get; init; }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<BoundCountryToCreate, T> create, Func<BoundCountryToUpdate, T> update);
    public abstract void Match(Action<BoundCountryToCreate> create, Action<BoundCountryToUpdate> update);

    public sealed record BoundCountryToCreate : BoundCountry, CountryToCreate, ISOCodedSubdivisionToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<BoundCountryToCreate, T> create, Func<BoundCountryToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<BoundCountryToCreate> create, Action<BoundCountryToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record BoundCountryToUpdate : BoundCountry, CountryToUpdate, ISOCodedSubdivisionToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<BoundCountryToCreate, T> create, Func<BoundCountryToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<BoundCountryToCreate> create, Action<BoundCountryToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record BoundCountryDetails
{
    public required int BindingCountryId { get; init; }
   
}

