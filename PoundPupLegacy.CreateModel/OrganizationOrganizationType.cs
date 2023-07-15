﻿namespace PoundPupLegacy.DomainModel;

public sealed record class OrganizationOrganizationType : IRequest
{
    public required int OrganizationId { get; set; }

    public required int OrganizationTypeId { get; init; }
}
