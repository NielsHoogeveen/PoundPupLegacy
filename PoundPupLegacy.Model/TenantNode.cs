namespace PoundPupLegacy.Model;

public record TenantNode
{
    public int TenantId { get; init; }

    public int UrlId { get; init; }

    public string? UrlPath { get; init; }

    public int? NodeId { get; set; }

    public int? SubgroupId { get; init; }

    public int PublicationStatusId { get; init; }

}
