namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(SingleQuestionPoll))]
public partial class SingleQuestionPollJsonContext : JsonSerializerContext { }

public sealed record class SingleQuestionPoll : PollQuestionBase, Poll
{
    public required string Question { get; init; }
    public required int PollStatusId { get; init; }
    public DateTime? DateTimeClosure { get; init; }
}
