namespace PoundPupLegacy.EditModel;

public record TenantNode
{
    public int? Id { get; init; }
    public required int TenantId { get; init; }
    public int? UrlId { get; init; }
    public required string? UrlPath { get; set; }
    public int? NodeId { get; init; }
    public int? SubgroupId { get; set; }
    public required int PublicationStatusId { get; set; }
    public required bool HasBeenStored { get; init; }
    public bool HasBeenDeleted { get; set; } = false;
    public bool CanBeUnchecked { get; set; } = true;
}
