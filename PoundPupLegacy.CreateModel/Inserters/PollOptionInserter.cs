namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PollOptionInserterFactory : DatabaseInserterFactory<PollOption, PollOptionInserter>
{
    internal static NonNullableIntegerDatabaseParameter PollQuesyionId = new() { Name = "poll_question_id" };
    internal static NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    internal static NonNullableIntegerDatabaseParameter NumberOfVotes = new() { Name = "number_of_votes" };

    public override string TableName => "poll_option";
}
internal sealed class PollOptionInserter : DatabaseInserter<PollOption>
{
    public PollOptionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(PollOption item)
    {
        if (item.PollQuestionId is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(PollOptionInserterFactory.PollQuesyionId, item.PollQuestionId.Value),
            ParameterValue.Create(PollOptionInserterFactory.Delta, item.Delta),
            ParameterValue.Create(PollOptionInserterFactory.Text, item.Text),
            ParameterValue.Create(PollOptionInserterFactory.NumberOfVotes, item.NumberOfVotes),
        };
    }
}
