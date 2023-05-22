namespace PoundPupLegacy.CreateModel;

public interface EventuallyIdentifiableNodeType : NodeType, EventuallyIdentifiable
{
}
public interface NodeType: IRequest
{
    string Name { get; }
    string Description { get; }
    bool AuthorSpecific { get; }
}
public abstract record NewNodeType: EventuallyIdentifiableNodeType
{
    public required int? Id { get; set; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required bool AuthorSpecific { get; init; }
}