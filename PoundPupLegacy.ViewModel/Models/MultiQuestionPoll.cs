namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(MemberOfCongress))]
public partial class MultiQuestionPollJsonContext : JsonSerializerContext { }

public sealed record class MultiQuestionPoll : PollBase
{
    private BasicPollQuestion[] pollQuestions = Array.Empty<BasicPollQuestion>();
    public BasicPollQuestion[] PollQuestions {
        get => pollQuestions;
        init {
            if (value is not null) {
                pollQuestions = value;
            }

        }
    }
}
