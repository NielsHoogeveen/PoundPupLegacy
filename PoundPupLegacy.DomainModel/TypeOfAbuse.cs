﻿namespace PoundPupLegacy.DomainModel;

public abstract record TypeOfAbuse : Nameable
{
    private TypeOfAbuse() { }
    public sealed record ToCreate : TypeOfAbuse, NameableToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : TypeOfAbuse, NameableToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}