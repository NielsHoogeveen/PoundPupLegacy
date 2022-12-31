﻿namespace PoundPupLegacy.Model;

public record NodeStatus : Identifiable
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
