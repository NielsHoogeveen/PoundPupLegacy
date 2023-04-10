namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PollQuestionInserterFactory : BasicDatabaseInserterFactory<PollQuestion, PollQuestionInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Question = new() { Name = "question" };

    public override string TableName => "poll_question";
}
internal sealed class PollQuestionInserter : BasicDatabaseInserter<PollQuestion>
{
    public PollQuestionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(PollQuestion item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(PollQuestionInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(PollQuestionInserterFactory.Question, item.Question),
        };
    }
}
