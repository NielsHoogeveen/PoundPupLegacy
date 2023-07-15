namespace PoundPupLegacy.DomainModel;

public abstract record BoundCountry : Country, ISOCodedSubdivision
{
    private BoundCountry() { }
    public required BoundCountryDetails BoundCountryDetails { get; init; }
    public required ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; init; }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public required CountryDetails CountryDetails { get; init; }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public sealed record ToCreate : BoundCountry, CountryToCreate, ISOCodedSubdivisionToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : BoundCountry, CountryToUpdate, ISOCodedSubdivisionToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}

public sealed record BoundCountryDetails
{
    public required int BindingCountryId { get; init; }

}

