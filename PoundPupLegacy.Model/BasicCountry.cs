﻿namespace PoundPupLegacy.Model;

public record BasicCountry : Node, TopLevelCountry
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int? OwnerId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int? FileIdTileImage { get; init; }
    public required List<VocabularyName> VocabularyNames { get; init; }
    public required int SecondLevelRegionId { get; init; }
    public required string ISO3166_1_Code { get; init; }
    public required int? FileIdFlag { get; init; }
    public required int HagueStatusId { get; init; }
    public required string? ResidencyRequirements { get; init; }
    public required string? AgeRequirements { get; init; }
    public required string? MarriageRequirements { get; init; }
    public required string? IncomeRequirements { get; init; }
    public required string? HealthRequirements { get; init; }
    public required string? OtherRequirements { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }
}


