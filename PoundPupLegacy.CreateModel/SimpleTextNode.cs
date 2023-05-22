namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiableSimpleTextNode : SimpleTextNode, ImmediatelyIdentifiableSearchable
{
}

public interface EventuallyIdentifiableSimpleTextNode : SimpleTextNode, EventuallyIdentifiableSearchable 
{ 
}

public interface SimpleTextNode : Searchable
{
    string Text { get; }

    string Teaser { get; }
}

public abstract record NewSimpleTextNodeBase: NewNodeBase, EventuallyIdentifiableSimpleTextNode 
{ 
    public required string Text { get; init; }
    public required string Teaser { get; init; }
}

public abstract record ExistingSimpleTextNodeBase : ExistingNodeBase, ImmediatelyIdentifiableSimpleTextNode
{
    public required string Text { get; init; }
    public required string Teaser { get; init; }
}
