namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SenateTermInserterFactory : DatabaseInserterFactory<SenateTerm>
{
    public override async Task<IDatabaseInserter<SenateTerm>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "senate_term",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = SenateTermInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SenateTermInserter.SENATOR_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SenateTermInserter.SUBDIVISION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SenateTermInserter.DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
            }
        );
        return new SenateTermInserter(command);
    }
}
internal sealed class SenateTermInserter : DatabaseInserter<SenateTerm>
{
    internal const string ID = "id";
    internal const string SENATOR_ID = "senator_id";
    internal const string SUBDIVISION_ID = "subdivision_id";
    internal const string DATE_RANGE = "date_range";

    internal SenateTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(SenateTerm senateTerm)
    {
        if (senateTerm.Id is null)
            throw new NullReferenceException();
        if (senateTerm.SenatorId is null)
            throw new NullReferenceException();
        WriteValue(senateTerm.Id, ID);
        WriteValue(senateTerm.SenatorId, SENATOR_ID);
        WriteValue(senateTerm.SubdivisionId, SUBDIVISION_ID);
        WriteDateTimeRange(senateTerm.DateTimeRange, DATE_RANGE);
        await _command.ExecuteNonQueryAsync();
    }
}
