﻿namespace PoundPupLegacy.DomainModel;

public abstract record DisruptedPlacementCase : Case
{
    private DisruptedPlacementCase() { }
    public sealed record ToCreate : DisruptedPlacementCase, CaseToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
        public required CaseDetails.CaseDetailsForCreate CaseDetails { get; init; }
        public required LocatableDetails.ForCreate LocatableDetails { get; init; }
    }
    public sealed record ToUpdate : DisruptedPlacementCase, CaseToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
        public required CaseDetails.CaseDetailsForUpdate CaseDetails { get; init; }
        public required LocatableDetails.ForUpdate LocatableDetails { get; init; }
    }
}
