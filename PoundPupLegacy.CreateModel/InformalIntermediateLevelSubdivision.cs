namespace PoundPupLegacy.CreateModel;

public abstract record InformalIntermediateLevelSubdivision : IntermediateLevelSubdivision
{
    private InformalIntermediateLevelSubdivision() { }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public sealed record ToCreate : InformalIntermediateLevelSubdivision, IntermediateLevelSubdivisionToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : InformalIntermediateLevelSubdivision, IntermediateLevelSubdivisionToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}
