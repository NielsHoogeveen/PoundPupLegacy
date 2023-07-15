namespace PoundPupLegacy.DomainModel;

public interface NodeTypeToAdd : NodeType, PossiblyIdentifiable
{
}
public interface NodeType : IRequest
{
    string Name { get; }
    string Description { get; }
    bool AuthorSpecific { get; }
}
public abstract record NewNodeType : NodeTypeToAdd
{
    public required Identification.Possible Identification { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required bool AuthorSpecific { get; init; }
}