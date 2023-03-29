namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PollStatusInserter : DatabaseInserter<PollStatus>, IDatabaseInserter<PollStatus>
{
    private const string ID = "id";
    private const string NAME = "name";
    public static async Task<DatabaseInserter<PollStatus>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
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

    internal override async Task WriteAsync(PollStatus pollStatus)
    {
        WriteValue(pollStatus.Id, ID);
        WriteValue(pollStatus.Name, NAME);
        await _command.ExecuteNonQueryAsync();
    }
}
