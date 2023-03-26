namespace PoundPupLegacy.Models;

public record TenantNode
{
    public required int TenantId { get; init; }
    public required int UrlId { get; init; }
    public required string UrlPath { get; init; }
}
