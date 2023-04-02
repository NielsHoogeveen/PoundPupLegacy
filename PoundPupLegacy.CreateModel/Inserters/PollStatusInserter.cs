namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PollStatusInserter : DatabaseInserter<PollStatus>, IDatabaseInserter<PollStatus>
{
    private const string ID = "id";
    private const string NAME = "name";
    public static async Task<DatabaseInserter<PollStatus>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll_status",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new PollStatusInserter(command);

    }

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
