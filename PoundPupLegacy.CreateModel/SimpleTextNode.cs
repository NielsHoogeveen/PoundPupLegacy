namespace PoundPupLegacy.CreateModel;
public interface SimpleTextNodeToUpdate : SimpleTextNode, SearchableToUpdate
{
}

public interface SimpleTextNodeToCreate : SimpleTextNode, SearchableToCreate 
{ 
}

public interface SimpleTextNode : Searchable
{
    SimpleTextNodeDetails SimpleTextNodeDetails { get; }
}

public sealed record SimpleTextNodeDetails
{ 
    public required string Text { get; init; }
    public required string Teaser { get; init; }
}

