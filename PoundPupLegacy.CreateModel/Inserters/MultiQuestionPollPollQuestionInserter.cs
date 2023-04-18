namespace PoundPupLegacy.CreateModel.Inserters;

using Request = MultiQuestionPollPollQuestion;

internal sealed class MultiQuestionPollPollQuestionInserterFactory : BasicDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter MultiQuestionPollId = new() { Name = "multi_question_poll_id" };
    internal static NonNullableIntegerDatabaseParameter PollQuesionId = new() { Name = "poll_question_id" };
    internal static NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };

    public override string TableName => "multi_question_poll_poll_question";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(MultiQuestionPollId, request.MultiQuestionPollId),
            ParameterValue.Create(PollQuesionId, request.PollQuestionId),
            ParameterValue.Create(Delta, request.Delta),
        };
    }
}
