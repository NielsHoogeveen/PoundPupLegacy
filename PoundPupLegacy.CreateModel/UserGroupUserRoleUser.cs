namespace PoundPupLegacy.CreateModel;

public sealed record UserGroupUserRoleUser
{
    public required int UserGroupId { get; init; }

    public required int UserRoleId { get; init; }

    public required int UserId { get; init; }
}
