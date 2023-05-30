namespace PoundPupLegacy.CreateModel;

public abstract record Vocabulary : Node
{
    private Vocabulary() { }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public required VocabularyDetails VocabularyDetails { get; init; }
    public sealed record ToCreate : Vocabulary, NodeToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
    }
    public sealed record ToUpdate : Vocabulary, NodeToUpdate
    {
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
    }
}
public sealed record VocabularyDetails
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int OwnerId { get; init; }
}
