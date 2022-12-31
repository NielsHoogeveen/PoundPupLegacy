namespace PoundPupLegacy.Model;

public record BindingCountry : Node, TopLevelCountry
{
    public required int Id { get; set; }
    public required int AccessRoleId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int NodeStatusId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Name { get; init; }
    public required int VocabularyId { get; init; }
    public required int GlobalRegionId { get; init; }
    public required string ISO3166_1_Code { get; init; }
    public required int? FileIdFlag { get; init; }
    public required int HagueStatusId { get; init; }
    public required string? ResidencyRequirements { get; init; }
    public required string? AgeRequirements { get; init; }
    public required string? MarriageRequirements { get; init; }
    public required string? IncomeRequirements { get; init; }
    public required string? HealthRequirements { get; init; }
    public required string? OtherRequirements { get; init; }
}


