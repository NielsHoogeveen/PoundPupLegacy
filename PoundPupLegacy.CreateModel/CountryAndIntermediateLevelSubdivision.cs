﻿namespace PoundPupLegacy.CreateModel;

public abstract record CountryAndIntermediateLevelSubdivision : CountryAndSubdivision, ISOCodedFirstLevelSubdivision, IntermediateLevelSubdivision, CountryAndFirstLevelSubdivision
{
    private CountryAndIntermediateLevelSubdivision() { }
    public required TopLevelCountryDetails TopLevelCountryDetails { get; init; }
    public required CountryDetails CountryDetails { get; init; }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public required ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; init; }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public sealed record ToCreate : CountryAndIntermediateLevelSubdivision, CountryAndSubdivisionToCreate, ISOCodedFirstLevelSubdivisionToCreate, IntermediateLevelSubdivisionToCreate, CountryAndFirstLevelSubdivisionToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
    }
    public sealed record ToUpdate : CountryAndIntermediateLevelSubdivision, CountryAndSubdivisionToUpdate, ISOCodedFirstLevelSubdivisionToUpdate, IntermediateLevelSubdivisionToUpdate, CountryAndFirstLevelSubdivisionToUpdate
    {
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
    }
}

