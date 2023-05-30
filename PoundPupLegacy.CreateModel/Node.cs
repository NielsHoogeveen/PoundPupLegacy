using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.CreateModel;

public interface NodeToUpdate : Node, ImmediatelyIdentifiable
{
    NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }

}
public interface NodeToCreate : Node, EventuallyIdentifiable
{
    NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
}
public interface Node: Identifiable 
{
    NodeDetails NodeDetails { get; }
}

public abstract record NodeDetails{
    private NodeDetails() { }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int AuthoringStatusId { get; init; }
    public abstract T Match<T>(Func<NodeDetailsForCreate, T> create, Func<NodeDetailsForUpdate, T> update);
    public abstract void Match(Action<NodeDetailsForCreate> create, Action<NodeDetailsForUpdate> update);
    public sealed record NodeDetailsForCreate: NodeDetails
    {
        public required int PublisherId { get; init; }
        public required DateTime CreatedDateTime { get; init; }
        public required int OwnerId { get; init; }
        public required int NodeTypeId { get; init; }
        public required List<TenantNode.TenantNodeToCreateForNewNode> TenantNodes { get; init; }
        public required List<int> TermIds { get; init; }
        public override T Match<T>(Func<NodeDetailsForCreate, T> create, Func<NodeDetailsForUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<NodeDetailsForCreate> create, Action<NodeDetailsForUpdate> update)
        {
            create(this);
        }
    }
    public sealed record NodeDetailsForUpdate : NodeDetails
    {
        public required List<TenantNode.TenantNodeToCreateForExistingNode> TenantNodesToAdd { get; init; }
        public required List<TenantNode.TenantNodeToUpdate> TenantNodesToUpdate { get; init; }
        public required List<TenantNodeToDelete> TenantNodesToRemove { get; init; }
        public required List<NodeTermToAdd> NodeTermsToAdd { get; init; }
        public required List<NodeTermToRemove> NodeTermsToRemove { get; init; }
        public override T Match<T>(Func<NodeDetailsForCreate, T> create, Func<NodeDetailsForUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<NodeDetailsForCreate> create, Action<NodeDetailsForUpdate> update)
        {
            update(this);
        }
    }
}