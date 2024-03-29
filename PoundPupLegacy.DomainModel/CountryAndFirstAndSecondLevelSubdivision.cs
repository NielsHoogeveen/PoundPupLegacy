﻿namespace PoundPupLegacy.DomainModel;

public abstract record CountryAndFirstAndSecondLevelSubdivision : CountryAndFirstLevelSubdivision, FirstAndSecondLevelSubdivision
{
    private CountryAndFirstAndSecondLevelSubdivision() { }
    public required TopLevelCountryDetails TopLevelCountryDetails { get; init; }
    public required CountryDetails CountryDetails { get; init; }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public required ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; init; }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public sealed record ToCreate : CountryAndFirstAndSecondLevelSubdivision, CountryAndFirstLevelSubdivisionToCreate, FirstAndSecondLevelSubdivisionToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : CountryAndFirstAndSecondLevelSubdivision, CountryAndFirstLevelSubdivisionToUpdate, FirstAndSecondLevelSubdivisionToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}

