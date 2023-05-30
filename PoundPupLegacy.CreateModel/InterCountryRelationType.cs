﻿namespace PoundPupLegacy.CreateModel;

public abstract record InterCountryRelationType : EndoRelationType
{
    private InterCountryRelationType() { }
    public required EndoRelationTypeDetails EndoRelationTypeDetails { get; init; }
    public sealed record ToCreate : InterCountryRelationType, EndoRelationTypeToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : InterCountryRelationType, EndoRelationTypeToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}
