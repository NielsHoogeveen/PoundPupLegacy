namespace PoundPupLegacy.Model;

public record BasicSecondLevelSubdivision : SecondLevelSubdivision
{
    public required int? Id { get; set; }
    public required int AccessRoleId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int NodeStatusId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<VocabularyName> VocabularyNames { get; init; }

    public required string ISO3166_2_Code { get; init; }
    public required int CountryId { get; init; }
    public required int IntermediateLevelSubdivisionId { get; init; }
    public required int? FileIdFlag { get; init; }
}
