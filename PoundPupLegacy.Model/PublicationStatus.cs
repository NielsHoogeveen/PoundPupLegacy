﻿namespace PoundPupLegacy.Model;

public record PublicationStatus : Identifiable
{
    public required int? Id { get; set; }
    public required string Name { get; init; }
}
