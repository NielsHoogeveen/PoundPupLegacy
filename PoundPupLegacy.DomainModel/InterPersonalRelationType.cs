﻿namespace PoundPupLegacy.DomainModel;

public abstract record InterPersonalRelationType : EndoRelationType
{
    private InterPersonalRelationType() { }
    public required EndoRelationTypeDetails EndoRelationTypeDetails { get; init; }
    public sealed record ToCreate : InterPersonalRelationType, EndoRelationTypeToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : InterPersonalRelationType, EndoRelationTypeToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}
