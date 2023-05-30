namespace PoundPupLegacy.CreateModel;

public abstract record FormalIntermediateLevelSubdivision : ISOCodedFirstLevelSubdivision, IntermediateLevelSubdivision
{
    private FormalIntermediateLevelSubdivision() { }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public required ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; init; }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public sealed record ToCreate : FormalIntermediateLevelSubdivision, ISOCodedFirstLevelSubdivisionToCreate, IntermediateLevelSubdivisionToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : FormalIntermediateLevelSubdivision, ISOCodedFirstLevelSubdivisionToUpdate, IntermediateLevelSubdivisionToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}

