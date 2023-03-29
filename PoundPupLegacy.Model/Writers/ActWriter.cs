namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class ActWriter : DatabaseWriter<Act>, IDatabaseWriter<Act>
{

    private const string ID = "id";
    private const string ENACTMENT_DATE = "enactment_date";
    public static async Task<DatabaseWriter<Act>> CreateAsync(NpgsqlConnection connection)
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
        return new ActWriter(command);

    }

    internal ActWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Act act)
    {
        WriteValue(act.Id, ID);
        WriteNullableValue(act.EnactmentDate, ENACTMENT_DATE);
        await _command.ExecuteNonQueryAsync();
    }
}
