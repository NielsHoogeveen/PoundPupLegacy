namespace PoundPupLegacy.CreateModel;

public abstract record BasicSecondLevelSubdivision : SecondLevelSubdivision
{
    private BasicSecondLevelSubdivision() { }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public required ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; init; }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public required int IntermediateLevelSubdivisionId { get; init; }
    public sealed record ToCreate : BasicSecondLevelSubdivision, FirstAndSecondLevelSubdivisionToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : BasicSecondLevelSubdivision, FirstAndSecondLevelSubdivisionToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}