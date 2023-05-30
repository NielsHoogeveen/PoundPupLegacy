namespace PoundPupLegacy.CreateModel;

public abstract record SubdivisionType : Nameable
{
    private SubdivisionType() { }
    public sealed record ToCreate : SubdivisionType, NameableToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : SubdivisionType, NameableToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}
