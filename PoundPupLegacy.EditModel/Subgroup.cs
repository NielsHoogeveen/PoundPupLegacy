﻿namespace PoundPupLegacy.EditModel;

public record Subgroup
{
    public required int Id { get; init; }
    public required string Name { get; init; }

    public required int PublicationStatusIdDefault { get; init; }
}
