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

    public sealed record ExistingTenantNode: TenantNode
    {
        public int Id { get; set; }
        public int UrlId { get; set; }
        public int NodeId { get; set; }
    }
    public sealed record NewTenantNodeForNewNode: TenantNode
    {

    }
    public sealed record NewTenantNodeForExistingNode: TenantNode
    {
        public int UrlId { get; set; }
        public int NodeId { get; set; }

    }
}
