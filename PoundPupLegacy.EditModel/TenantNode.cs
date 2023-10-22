namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TenantNode.ToUpdate))]
public partial class ExistingTenantNodeJsonContext : JsonSerializerContext { }

public abstract record TenantNode
{
    public required int TenantId { get; set; }
    public required string? UrlPath { get; set; }
    public int? SubgroupId { get; set; }
    public required int PublicationStatusId { get; set; }
    public required bool HasBeenStored { get; set; }
    public bool HasBeenDeleted { get; set; } = false;
    public bool CanBeUnchecked { get; set; } = true;

    public abstract void Match(
        Action<ToUpdate> existingTenantNode,
        Action<ToCreateForExistingNode> newTenantNodeForExistingNode,
        Action<ToCreateForNewNode> newTenantNodeForNewNode
    );

    public sealed record ToUpdate: TenantNode
    {
        public int Id { get; set; }
        public int UrlId { get; set; }
        public int NodeId { get; set; }
        public override void Match(
            Action<ToUpdate> existingTenantNode,
            Action<ToCreateForExistingNode> newTenantNodeForExistingNode,
            Action<ToCreateForNewNode> newTenantNodeForNewNode
        )
        {
            existingTenantNode(this);
        }
    }
    public sealed record ToCreateForNewNode: TenantNode
    {
        public override void Match(
            Action<ToUpdate> existingTenantNode,
            Action<ToCreateForExistingNode> newTenantNodeForExistingNode,
            Action<ToCreateForNewNode> newTenantNodeForNewNode
        )
        {
            newTenantNodeForNewNode(this);
        }

    }
    public sealed record ToCreateForExistingNode: TenantNode
    {
        public int UrlId { get; set; }
        public int NodeId { get; set; }
        public override void Match(
            Action<ToUpdate> existingTenantNode,
            Action<ToCreateForExistingNode> newTenantNodeForExistingNode,
            Action<ToCreateForNewNode> newTenantNodeForNewNode
        )
        {
            newTenantNodeForExistingNode(this);
        }

    }
}
