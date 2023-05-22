﻿namespace PoundPupLegacy.CreateModel;

public sealed record CaseParties : EventuallyIdentifiable
{
    public required int? Id { get; set; }

    public required string? Organizations { get; init; }
    public required string? Persons { get; init; }

    public required List<int> OrganizationIds { get; init; }

    public required List<int> PersonsIds { get; init; }

}
