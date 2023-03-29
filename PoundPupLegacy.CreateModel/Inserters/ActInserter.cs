namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ActInserter : DatabaseInserter<Act>, IDatabaseInserter<Act>
{

    private const string ID = "id";
    private const string ENACTMENT_DATE = "enactment_date";
    public static async Task<DatabaseInserter<Act>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "act",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ENACTMENT_DATE,
                    NpgsqlDbType = NpgsqlDbType.Date
                },
            }
        );
        return new ActInserter(command);

    }

    internal ActInserter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Act act)
    {
        WriteValue(act.Id, ID);
        WriteNullableValue(act.EnactmentDate, ENACTMENT_DATE);
        await _command.ExecuteNonQueryAsync();
    }
}
