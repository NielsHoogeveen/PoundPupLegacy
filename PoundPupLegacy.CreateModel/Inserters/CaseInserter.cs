namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CaseInserter : DatabaseInserter<Case>, IDatabaseInserter<Case>
{
    private const string ID = "id";
    private const string DESCRIPTION = "description";
    private const string DATE = "date";
    private const string DATERANGE = "date_range";
    public static async Task<DatabaseInserter<Case>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "case",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = DATE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
                new ColumnDefinition{
                    Name = DATERANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
            }
        );
        return new CaseInserter(command);

    }

    internal CaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Case @case)
    {
        if (@case.Id is null)
            throw new NullReferenceException();
        WriteValue(@case.Id, ID);
        WriteValue(@case.Description, DESCRIPTION);
        WriteDateTimeRange(@case.Date, DATE, DATERANGE);
        await _command.ExecuteNonQueryAsync();
    }
}
