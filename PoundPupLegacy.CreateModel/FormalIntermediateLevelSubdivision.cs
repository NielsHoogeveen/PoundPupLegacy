namespace PoundPupLegacy.CreateModel;

public sealed record FormalIntermediateLevelSubdivision : ISOCodedFirstLevelSubdivision, IntermediateLevelSubdivision
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int OwnerId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int? FileIdTileImage { get; init; }
    public required List<VocabularyName> VocabularyNames { get; init; }
    public required int CountryId { get; init; }
    public required string ISO3166_2_Code { get; init; }
    public required int? FileIdFlag { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }
    public required int SubdivisionTypeId { get; init; }

}
