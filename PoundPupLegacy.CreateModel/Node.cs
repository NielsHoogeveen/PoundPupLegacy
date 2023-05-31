using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.CreateModel;

public interface NodeToUpdate : Node, CertainlyIdentifiable
{
    NodeDetails.ForUpdate NodeDetails { get; init; }

}
public interface NodeToCreate : Node, PossiblyIdentifiable
{
    NodeDetails.ForCreate NodeDetails { get; init; }
}
public interface Node: Identifiable 
{
}

public abstract record NodeDetails{
    private NodeDetails() { }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int AuthoringStatusId { get; init; }
    public sealed record ForCreate: NodeDetails
    {
        public required int PublisherId { get; init; }
        public required DateTime CreatedDateTime { get; init; }
        public required int OwnerId { get; init; }
        public required int NodeTypeId { get; init; }
        public required List<TenantNode.ToCreate.ForNewNode> TenantNodes { get; init; }
        public required List<int> TermIds { get; init; }
    }
    public sealed record ForUpdate : NodeDetails
    {
        public required List<TenantNode.ToCreate.ForExistingNode> TenantNodesToAdd { get; init; }
        public required List<TenantNode.ToUpdate> TenantNodesToUpdate { get; init; }
        public required List<TenantNodeToDelete> TenantNodesToRemove { get; init; }
        public required List<ResolvedNodeTermToAdd> NodeTermsToAdd { get; init; }
        public required List<NodeTermToRemove> NodeTermsToRemove { get; init; }
    }
}