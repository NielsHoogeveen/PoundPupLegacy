namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TenantNode))]
public partial class TenantNodeJsonContext : JsonSerializerContext { }

public record TenantNode
{
    public int? Id { get; set; }
    public required int TenantId { get; set; }
    public int? UrlId { get; set; }
    public required string? UrlPath { get; set; }
    public int? NodeId { get; set; }
    public int? SubgroupId { get; set; }
    public required int PublicationStatusId { get; set; }
    public required bool HasBeenStored { get; set; }
    public bool HasBeenDeleted { get; set; } = false;
    public bool CanBeUnchecked { get; set; } = true;
}
