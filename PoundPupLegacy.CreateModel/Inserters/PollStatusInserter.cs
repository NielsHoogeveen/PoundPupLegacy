namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PollStatusInserterFactory : DatabaseInserterFactory<PollStatus>
{
    public override async Task<IDatabaseInserter<PollStatus>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll_status",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = PollStatusInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PollStatusInserter.NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new PollStatusInserter(command);
    }
}
internal sealed class PollStatusInserter : DatabaseInserter<PollStatus>
{
    internal const string ID = "id";
    internal const string NAME = "name";

    internal PollStatusInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PollStatus pollStatus)
    {
        WriteValue(pollStatus.Id, ID);
        WriteValue(pollStatus.Name, NAME);
        await _command.ExecuteNonQueryAsync();
    }
}
