namespace PoundPupLegacy.Model;

public record DeportationCase : Case
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int? OwnerId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Description { get; init; }
    public required int? FileIdTileImage { get; init; }
    public required List<VocabularyName> VocabularyNames { get; init; }
    public required DateTimeRange? Date { get; init; }
    public required int? SubdivisionIdFrom { get; init; }
    public required int? CountryIdTo { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }

}
