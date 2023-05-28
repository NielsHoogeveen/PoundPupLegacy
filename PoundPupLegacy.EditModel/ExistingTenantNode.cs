namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TenantNode.ExistingTenantNode))]
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
        Action<ExistingTenantNode> existingTenantNode,
        Action<NewTenantNodeForExistingNode> newTenantNodeForExistingNode,
        Action<NewTenantNodeForNewNode> newTenantNodeForNewNode
    );

    public sealed record ExistingTenantNode: TenantNode
    {
        public int Id { get; set; }
        public int UrlId { get; set; }
        public int NodeId { get; set; }
        public override void Match(
            Action<ExistingTenantNode> existingTenantNode,
            Action<NewTenantNodeForExistingNode> newTenantNodeForExistingNode,
            Action<NewTenantNodeForNewNode> newTenantNodeForNewNode
        )
        {
            existingTenantNode(this);
        }
    }
    public sealed record NewTenantNodeForNewNode: TenantNode
    {
        public override void Match(
            Action<ExistingTenantNode> existingTenantNode,
            Action<NewTenantNodeForExistingNode> newTenantNodeForExistingNode,
            Action<NewTenantNodeForNewNode> newTenantNodeForNewNode
        )
        {
            newTenantNodeForNewNode(this);
        }

    }
    public sealed record NewTenantNodeForExistingNode: TenantNode
    {
        public int UrlId { get; set; }
        public int NodeId { get; set; }
        public override void Match(
            Action<ExistingTenantNode> existingTenantNode,
            Action<NewTenantNodeForExistingNode> newTenantNodeForExistingNode,
            Action<NewTenantNodeForNewNode> newTenantNodeForNewNode
        )
        {
            newTenantNodeForExistingNode(this);
        }

    }
}
