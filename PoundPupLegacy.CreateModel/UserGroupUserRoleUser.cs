﻿namespace PoundPupLegacy.CreateModel;

public sealed record UserGroupUserRoleUser: IRequest
{
    public required int UserGroupId { get; init; }

    public required int UserRoleId { get; init; }

    public required int UserId { get; init; }
}
