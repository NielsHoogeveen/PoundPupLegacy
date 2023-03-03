namespace PoundPupLegacy.EditModel;

public record TenantNode
{
    public required int? Id { get; init; }
    public required int TenantId { get; init; }
    public required int UrlId { get; init; }
    public required string? UrlPath { get; set; }
    public required int NodeId { get; init; }
    public required int? SubgroupId { get; set; }
    public required int PublicationStatusId { get; set; }
    public bool HasBeenDeleted { get; set; } = false;
}
