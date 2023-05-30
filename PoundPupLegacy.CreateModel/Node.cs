using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.CreateModel;

public interface NodeToUpdate : Node, CertainlyIdentifiable
{
    NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }

}
public interface NodeToCreate : Node, PossiblyIdentifiable
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
    public abstract T Match<T>(Func<NodeDetailsForCreate, T> create, Func<ForUpdate, T> update);
    public abstract void Match(Action<NodeDetailsForCreate> create, Action<ForUpdate> update);
    public sealed record NodeDetailsForCreate: NodeDetails
    {
        public required int PublisherId { get; init; }
        public required DateTime CreatedDateTime { get; init; }
        public required int OwnerId { get; init; }
        public required int NodeTypeId { get; init; }
        public required List<TenantNode.ToCreateForNewNode> TenantNodes { get; init; }
        public required List<int> TermIds { get; init; }
        public override T Match<T>(Func<NodeDetailsForCreate, T> create, Func<ForUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<NodeDetailsForCreate> create, Action<ForUpdate> update)
        {
            create(this);
        }
    }
    public sealed record ForUpdate : NodeDetails
    {
        public required List<TenantNode.ToCreateForExistingNode> TenantNodesToAdd { get; init; }
        public required List<TenantNode.ToUpdate> TenantNodesToUpdate { get; init; }
        public required List<TenantNodeToDelete> TenantNodesToRemove { get; init; }
        public required List<NodeTermToAdd> NodeTermsToAdd { get; init; }
        public required List<NodeTermToRemove> NodeTermsToRemove { get; init; }
        public override T Match<T>(Func<NodeDetailsForCreate, T> create, Func<ForUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<NodeDetailsForCreate> create, Action<ForUpdate> update)
        {
            update(this);
        }
    }
}