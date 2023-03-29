namespace PoundPupLegacy.CreateModel;

public record MultiQuestionPoll : SimpleTextNode, Poll
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int? OwnerId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Text { get; set; }
    public required string Teaser { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }

    public required int PollStatusId { get; init; }
    public required DateTime DateTimeClosure { get; init; }

    public required List<BasicPollQuestion> PollQuestions { get; init; }

}
