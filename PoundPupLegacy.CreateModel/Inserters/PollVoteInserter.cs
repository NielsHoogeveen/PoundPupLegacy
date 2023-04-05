namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PollVoteInserterFactory : DatabaseInserterFactory<PollVote> 
{
    public override async Task<IDatabaseInserter<PollVote>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll_vote",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = PollVoteInserter.POLL_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PollVoteInserter.DELTA,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PollVoteInserter.USER_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PollVoteInserter.IP_ADDRESS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new PollVoteInserter(command);
    }
}
internal sealed class PollVoteInserter : DatabaseInserter<PollVote>
{
    internal const string POLL_ID = "poll_id";
    internal const string DELTA = "delta";
    internal const string USER_ID = "user_id";
    internal const string IP_ADDRESS = "ip_address";

    internal PollVoteInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PollVote pollVote)
    {
        if (pollVote.PollId is null)
            throw new NullReferenceException();
        WriteValue(pollVote.PollId, POLL_ID);
        WriteValue(pollVote.Delta, DELTA);
        WriteNullableValue(pollVote.UserId, USER_ID);
        WriteNullableValue(pollVote.IpAddress, IP_ADDRESS);
        await _command.ExecuteNonQueryAsync();
    }
}
