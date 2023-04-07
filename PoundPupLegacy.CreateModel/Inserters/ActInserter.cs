namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ActInserterFactory : DatabaseInserterFactory<Act>
{
    public override async Task<IDatabaseInserter<Act>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "act",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ActInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ActInserter.ENACTMENT_DATE,
                    NpgsqlDbType = NpgsqlDbType.Date
                },
            }
        );
        return new ActInserter(command);

    }
}
internal sealed class ActInserter : DatabaseInserter<Act>
{

    public const string ID = "id";
    public const string ENACTMENT_DATE = "enactment_date";

    internal ActInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Act act)
    {
        SetParameter(act.Id, ID);
        SetNullableParameter(act.EnactmentDate, ENACTMENT_DATE);
        await _command.ExecuteNonQueryAsync();
    }
}
