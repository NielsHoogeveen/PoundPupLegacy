namespace PoundPupLegacy.Model;

public record UserGroupUser
{
    public required int UserGroupId { get; init; }

    public required int UserId { get; init; }
}
