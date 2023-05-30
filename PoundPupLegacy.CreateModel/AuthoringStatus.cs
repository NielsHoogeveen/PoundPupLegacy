﻿namespace PoundPupLegacy.CreateModel;

public sealed record AuthoringStatus : EventuallyIdentifiable
{
    public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
    public Identification Identification => IdentificationForCreate;

    public required string Name { get; init; }
}
