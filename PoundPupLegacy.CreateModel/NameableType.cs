namespace PoundPupLegacy.CreateModel;

public interface IdentifiableNameableType : NameableType, EventuallyIdentifiableNodeType
{

}
public interface NameableType : NodeType
{
    string TagLabelName { get; }
}

public abstract record NameableTypeBase: NewNodeType, NameableType
{
    public required string TagLabelName { get; init; }
}
