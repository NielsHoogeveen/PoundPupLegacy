using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.CreateModel;

public interface NodeToUpdate : Node, IRequest
{
    NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
    NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }

}
public interface NodeToCreate : Node, IRequest
{
    NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
    NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
}
public interface Node: IRequest 
{
    NodeIdentification NodeIdentification { get; }
    NodeDetails NodeDetails { get; }
}

public abstract record NodeIdentification
{
    private NodeIdentification() { }
    public abstract T Match<T>(Func<NodeIdentificationForCreate, T> create, Func<NodeIdentificationForUpdate, T> update);
    public abstract void Match(Action<NodeIdentificationForCreate> create, Action<NodeIdentificationForUpdate> update);
    public sealed record NodeIdentificationForCreate : NodeIdentification
    {
        public required int? Id { get; init; }
        public override T Match<T>(Func<NodeIdentificationForCreate, T> create, Func<NodeIdentificationForUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<NodeIdentificationForCreate> create, Action<NodeIdentificationForUpdate> update)
        {
            create(this);
        }
    }
    public sealed record NodeIdentificationForUpdate : NodeIdentification
    { 
        public required int Id { get; init; }
        public override T Match<T>(Func<NodeIdentificationForCreate, T> create, Func<NodeIdentificationForUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<NodeIdentificationForCreate> create, Action<NodeIdentificationForUpdate> update)
        {
            update(this);
        }
    }
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
        public required List<NewTenantNodeForNewNode> TenantNodes { get; init; }
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
        public required List<NewTenantNodeForExistingNode> TenantNodesToAdd { get; init; }
        public required List<ExistingTenantNode> TenantNodesToUpdate { get; init; }
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