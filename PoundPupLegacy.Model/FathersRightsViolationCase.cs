﻿namespace PoundPupLegacy.Model;

public record FathersRightsViolationCase : Case
{
    public required int Id { get; set; }
    public required int AccessRoleId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int NodeStatusId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Description { get; init; }
    public required DateTimeRange? Date { get; init; }
    public required bool IsTopic { get; init; }


}