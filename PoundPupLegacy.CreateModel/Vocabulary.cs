namespace PoundPupLegacy.CreateModel;

public sealed record NewVocabulary : NewNodeBase, EventuallyIdentifiableVocabulary
{
   public required string Name { get; init; }
    public required string Description { get; init; }
}
public sealed record ExistingVocabulary : ExistingNodeBase, ImmediatelyIdentifiableVocabulary
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}
public interface ImmediatelyIdentifiableVocabulary : Vocabulary, ImmediatelyIdentifiableNode
{
}
public interface EventuallyIdentifiableVocabulary : Vocabulary, EventuallyIdentifiableNode
{
}
public interface Vocabulary : Node
{
    string Name { get; }
    string Description { get; }
}
