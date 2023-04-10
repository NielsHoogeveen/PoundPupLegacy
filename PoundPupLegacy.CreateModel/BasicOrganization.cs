namespace PoundPupLegacy.CreateModel;

public sealed record BasicOrganization : Organization
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int OwnerId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string? WebsiteUrl { get; init; }
    public required string? EmailAddress { get; init; }
    public required string Description { get; init; }
    public required DateTimeRange? Established { get; init; }
    public required DateTimeRange? Terminated { get; init; }
    public required List<VocabularyName> VocabularyNames { get; init; }
    public required int? FileIdTileImage { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }
    public required List<OrganizationOrganizationType> OrganizationTypes { get; set; }

}
