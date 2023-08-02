﻿namespace PoundPupLegacy.DomainModel;

public sealed record class AccessRolePrivilege : IRequest
{
    public required int AccessRoleId { get; init; }

    public required int ActionId { get; init; }
}