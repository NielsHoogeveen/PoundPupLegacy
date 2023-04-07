namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CaseInserterFactory : DatabaseInserterFactory<Case>
{
    public override async Task<IDatabaseInserter<Case>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "case",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CaseInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CaseInserter.DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = CaseInserter.DATE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
                new ColumnDefinition{
                    Name = CaseInserter.DATERANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
            }
        );
        return new CaseInserter(command);

    }

}
internal sealed class CaseInserter : DatabaseInserter<Case>
{
    internal const string ID = "id";
    internal const string DESCRIPTION = "description";
    internal const string DATE = "date";
    internal const string DATERANGE = "date_range";
    
    internal CaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Case @case)
    {
        if (@case.Id is null)
            throw new NullReferenceException();
        SetParameter(@case.Id, ID);
        SetParameter(@case.Description, DESCRIPTION);
        SetDateTimeRangeParameter(@case.Date, DATE, DATERANGE);
        await _command.ExecuteNonQueryAsync();
    }
}
