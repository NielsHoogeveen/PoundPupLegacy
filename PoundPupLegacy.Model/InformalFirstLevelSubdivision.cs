﻿namespace PoundPupLegacy.Model;

public record InformalFirstLevelSubdivision : FirstLevelSubdivision
{
    public required int Id { get; set; }
    public required int UserId { get; init; }
    public required DateTime Created { get; init; }
    public required DateTime Changed { get; init; }
    public required string Title { get; init; }
    public required int Status { get; init; }
    public required int NodeTypeId { get; init; }
    public required bool IsTerm { get; init; }
    public required string Name { get; init; }
    public required int CountryId { get; init; }
}
