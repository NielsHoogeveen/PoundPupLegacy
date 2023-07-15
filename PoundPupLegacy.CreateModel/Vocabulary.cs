namespace PoundPupLegacy.DomainModel;

public abstract record Vocabulary : Node
{
    private Vocabulary() { }
    public required VocabularyDetails VocabularyDetails { get; init; }
    public sealed record ToCreate : Vocabulary, NodeToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
    }
    public sealed record ToUpdate : Vocabulary, NodeToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}
public sealed record VocabularyDetails
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int OwnerId { get; init; }
}
