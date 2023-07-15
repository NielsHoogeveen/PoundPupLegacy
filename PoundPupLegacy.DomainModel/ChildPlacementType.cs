namespace PoundPupLegacy.DomainModel;

public abstract record ChildPlacementType : Nameable
{
    private ChildPlacementType() { }
    public sealed record ToCreate : ChildPlacementType, NameableToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : ChildPlacementType, NameableToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}
