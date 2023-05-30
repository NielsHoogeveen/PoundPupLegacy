namespace PoundPupLegacy.CreateModel;

public abstract record FirstAndBottomLevelSubdivision : ISOCodedFirstLevelSubdivision, BottomLevelSubdivision
{
    private FirstAndBottomLevelSubdivision() { }
    public required ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; init; }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public required SubdivisionDetails SubdivisionDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public sealed record ToCreate : FirstAndBottomLevelSubdivision, ISOCodedFirstLevelSubdivisionToCreate, BottomLevelSubdivisionToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
    }
    public sealed record FirstAndBottomLevelSubdivisionToUpdate : FirstAndBottomLevelSubdivision, ISOCodedFirstLevelSubdivisionToUpdate, BottomLevelSubdivisionToUpdate
    {
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
    }
}

