namespace PoundPupLegacy.DomainModel;

public sealed record PollVote : IRequest
{
    public required int? PollId { get; set; }
    public required int Delta { get; init; }
    public required int? UserId { get; init; }
    public required string? IpAddress { get; init; }
}
