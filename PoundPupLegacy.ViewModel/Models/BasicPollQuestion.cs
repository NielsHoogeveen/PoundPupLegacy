namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BasicPollQuestion))]
public partial class BasicPollQuestionJsonContext : JsonSerializerContext { }

public sealed record BasicPollQuestion : PollQuestionBase
{
   
}
