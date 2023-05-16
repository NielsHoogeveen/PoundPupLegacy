﻿namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(OrganizationType))]
public partial class OrganizationTypeJsonContext : JsonSerializerContext { }

public sealed record OrganizationType
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public required bool HasConcreteSubtype { get; init; }
}
