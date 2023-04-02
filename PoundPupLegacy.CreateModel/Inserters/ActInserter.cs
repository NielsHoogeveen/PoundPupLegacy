namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ActInserter : DatabaseInserter<Act>, IDatabaseInserter<Act>
{

    private const string ID = "id";
    private const string ENACTMENT_DATE = "enactment_date";
    public static async Task<DatabaseInserter<Act>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
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

    public override async Task InsertAsync(Act act)
    {
        WriteValue(act.Id, ID);
        WriteNullableValue(act.EnactmentDate, ENACTMENT_DATE);
        await _command.ExecuteNonQueryAsync();
    }
}
