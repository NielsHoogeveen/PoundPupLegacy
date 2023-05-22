namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableSearchable : Searchable, ImmediatelyIdentifiableNode
{
}

public interface EventuallyIdentifiableSearchable: Searchable, EventuallyIdentifiableNode
{
}

public interface Searchable : Node
{
}
