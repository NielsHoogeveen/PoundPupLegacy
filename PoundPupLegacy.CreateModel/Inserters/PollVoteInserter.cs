namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PollVoteInserter : DatabaseInserter<PollVote>, IDatabaseInserter<PollVote>
{
    private const string POLL_ID = "poll_id";
    private const string DELTA = "delta";
    private const string USER_ID = "user_id";
    private const string IP_ADDRESS = "ip_address";
    public static async Task<DatabaseInserter<PollVote>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll_vote",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = POLL_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DELTA,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = USER_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = IP_ADDRESS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new PollVoteInserter(command);

    }

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
