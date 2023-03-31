﻿namespace PoundPupLegacy.CreateModel;

public sealed record PublicationStatus : Identifiable
{
    public required int? Id { get; set; }
    public required string Name { get; init; }
}