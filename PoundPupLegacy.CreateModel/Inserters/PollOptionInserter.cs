namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PollOptionInserterFactory : DatabaseInserterFactory<PollOption>
{
    internal static NonNullableIntegerDatabaseParameter PollQuesyionId = new() { Name = "poll_question_id" };
    internal static NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    internal static NonNullableIntegerDatabaseParameter NumberOfVotes = new() { Name = "number_of_votes" };

    public override async Task<IDatabaseInserter<PollOption>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll_option",
            new DatabaseParameter[] {
                PollQuesyionId,
                Delta,
                Text,
                NumberOfVotes
            }
        );
        return new PollOptionInserter(command);
    }
}
internal sealed class PollOptionInserter : DatabaseInserter<PollOption>
{
    internal PollOptionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PollOption pollOption)
    {
        if (pollOption.PollQuestionId is null)
            throw new NullReferenceException();
        Set(PollOptionInserterFactory.PollQuesyionId, pollOption.PollQuestionId.Value);
        Set(PollOptionInserterFactory.Delta, pollOption.Delta);
        Set(PollOptionInserterFactory.Text, pollOption.Text);
        Set(PollOptionInserterFactory.NumberOfVotes, pollOption.NumberOfVotes);
        await _command.ExecuteNonQueryAsync();
    }
}
