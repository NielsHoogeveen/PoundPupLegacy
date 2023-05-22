namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableNode : Node, ImmediatelyIdentifiable
{

}
public interface EventuallyIdentifiableNode : Node, EventuallyIdentifiable
{
    public int PublisherId { get; }
    public DateTime CreatedDateTime { get; }
    public int OwnerId { get; }
    public int NodeTypeId { get; }

}
public interface Node: IRequest 
{

    public DateTime ChangedDateTime { get; }
    public string Title { get; }
    public List<TenantNode> TenantNodes { get; }
    public int AuthoringStatusId { get; }

}

public abstract record NewNodeBase: EventuallyIdentifiableNode
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int OwnerId { get; init; }
    public required int AuthoringStatusId { get; init; }
    public required int NodeTypeId { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }

}
public abstract record ExistingNodeBase : ImmediatelyIdentifiableNode
{
    public required int Id { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int AuthoringStatusId { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }

}
