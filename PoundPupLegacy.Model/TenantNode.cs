namespace PoundPupLegacy.Model;

public sealed record TenantNode : Identifiable
{
    public required int? Id { get; set; }
    public required int TenantId { get; init; }

    public required int? UrlId { get; init; }

    public required string? UrlPath { get; init; }

    public required int? NodeId { get; set; }

    public required int? SubgroupId { get; init; }

    public required int PublicationStatusId { get; init; }

}
