using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableNode : Node, ImmediatelyIdentifiable
{
    public List<NewTenantNodeForExistingNode> TenantNodesToAdd { get; }
    public List<ExistingTenantNode> TenantNodesToUpdate { get; }
    public List<TenantNodeToDelete> TenantNodesToRemove { get; }
    List<NodeTermToAdd> NodeTermsToAdd { get; }
    List<NodeTermToRemove> NodeTermsToRemove { get; }
}
public interface EventuallyIdentifiableNode : Node, EventuallyIdentifiable
{
    int PublisherId { get; }
    DateTime CreatedDateTime { get; }
    int OwnerId { get; }
    int NodeTypeId { get; }
    List<NewTenantNodeForNewNode> TenantNodes { get; }
    List<int> TermIds { get; }
}
public interface Node: IRequest 
{
    public DateTime ChangedDateTime { get; }
    public string Title { get; }
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
    public required List<NewTenantNodeForNewNode> TenantNodes { get; init; }
    public required List<int> TermIds { get; init; }

}
public abstract record ExistingNodeBase : ImmediatelyIdentifiableNode
{
    public required int Id { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int AuthoringStatusId { get; init; }
    public required List<NewTenantNodeForExistingNode> TenantNodesToAdd { get; init; }
    public required List<ExistingTenantNode> TenantNodesToUpdate { get; init; }
    public required List<TenantNodeToDelete> TenantNodesToRemove { get; init; }
    public required List<NodeTermToAdd> NodeTermsToAdd { get; init; }
    public required List<NodeTermToRemove> NodeTermsToRemove { get; init; }
}
