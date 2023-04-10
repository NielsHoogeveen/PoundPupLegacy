namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PollVoteInserterFactory : DatabaseInserterFactory<PollVote> 
{
    internal static NonNullableIntegerDatabaseParameter PollId = new() { Name = "poll_id" };
    internal static NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };
    internal static NullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };
    internal static NullableStringDatabaseParameter IPAddress = new() { Name = "ip_address" };

    public override async Task<IDatabaseInserter<PollVote>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll_vote",
            new DatabaseParameter[] {
                PollId,
                Delta,
                UserId,
                IPAddress
            }
        );
        return new PollVoteInserter(command);
    }
}
internal sealed class PollVoteInserter : DatabaseInserter<PollVote>
{

    internal PollVoteInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PollVote pollVote)
    {
        if (pollVote.PollId is null)
            throw new NullReferenceException();
        Set(PollVoteInserterFactory.PollId, pollVote.PollId.Value);
        Set(PollVoteInserterFactory.Delta, pollVote.Delta);
        Set(PollVoteInserterFactory.UserId, pollVote.UserId);
        Set(PollVoteInserterFactory.IPAddress, pollVote.IpAddress);
        await _command.ExecuteNonQueryAsync();
    }
}
