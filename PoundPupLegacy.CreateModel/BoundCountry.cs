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
    public sealed record ToCreate : BoundCountry, CountryToCreate, ISOCodedSubdivisionToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
    }
    public sealed record ToUpdate : BoundCountry, CountryToUpdate, ISOCodedSubdivisionToUpdate
    {
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
    }
}

public sealed record BoundCountryDetails
{
    public required int BindingCountryId { get; init; }
   
}

