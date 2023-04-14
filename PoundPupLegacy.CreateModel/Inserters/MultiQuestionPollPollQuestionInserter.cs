namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class MultiQuestionPollPollQuestionInserterFactory : DatabaseInserterFactory<MultiQuestionPollPollQuestion, MultiQuestionPollPollQuestionInserter>
{
    internal static NonNullableIntegerDatabaseParameter MultiQuestionPollId = new() { Name = "multi_question_poll_id" };
    internal static NonNullableIntegerDatabaseParameter PollQuesionId = new() { Name = "poll_question_id" };
    internal static NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };

    public override string TableName => "multi_question_poll_poll_question";
}
internal sealed class MultiQuestionPollPollQuestionInserter : DatabaseInserter<MultiQuestionPollPollQuestion>
{
    public MultiQuestionPollPollQuestionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(MultiQuestionPollPollQuestion item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(MultiQuestionPollPollQuestionInserterFactory.MultiQuestionPollId, item.MultiQuestionPollId),
            ParameterValue.Create(MultiQuestionPollPollQuestionInserterFactory.PollQuesionId, item.PollQuestionId),
            ParameterValue.Create(MultiQuestionPollPollQuestionInserterFactory.Delta, item.Delta),
        };
    }
}
