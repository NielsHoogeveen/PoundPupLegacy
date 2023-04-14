namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = MultiQuestionPollPollQuestionInserterFactory;
using Request = MultiQuestionPollPollQuestion;
using Inserter = MultiQuestionPollPollQuestionInserter;

internal sealed class MultiQuestionPollPollQuestionInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter MultiQuestionPollId = new() { Name = "multi_question_poll_id" };
    internal static NonNullableIntegerDatabaseParameter PollQuesionId = new() { Name = "poll_question_id" };
    internal static NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };

    public override string TableName => "multi_question_poll_poll_question";
}
internal sealed class MultiQuestionPollPollQuestionInserter : DatabaseInserter<Request>
{
    public MultiQuestionPollPollQuestionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.MultiQuestionPollId, request.MultiQuestionPollId),
            ParameterValue.Create(Factory.PollQuesionId, request.PollQuestionId),
            ParameterValue.Create(Factory.Delta, request.Delta),
        };
    }
}
