namespace PoundPupLegacy.CreateModel;

public abstract record BasicFirstAndSecondLevelSubdivision : FirstAndSecondLevelSubdivision
{
    private BasicFirstAndSecondLevelSubdivision() { }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public required ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; init; }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public sealed record ToCreate : BasicFirstAndSecondLevelSubdivision, FirstAndSecondLevelSubdivisionToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : BasicFirstAndSecondLevelSubdivision, FirstAndSecondLevelSubdivisionToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}



