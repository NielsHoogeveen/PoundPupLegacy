namespace PoundPupLegacy.CreateModel;

public class BasicPollQuestion : PollQuestion
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int OwnerId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Text { get; set; }
    public required string Teaser { get; init; }
    public required string Question { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }
    public required List<PollOption> PollOptions { get; init; }
    public required List<PollVote> PollVotes { get; init; }

}
