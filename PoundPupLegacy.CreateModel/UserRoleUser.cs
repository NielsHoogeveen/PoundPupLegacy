namespace PoundPupLegacy.CreateModel;

public sealed record UserRoleUser : IRequest
{
    public required int UserRoleId { get; init; }
    public required int UserId { get; init; }
}
