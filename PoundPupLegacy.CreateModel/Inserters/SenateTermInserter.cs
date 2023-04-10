namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SenateTermInserterFactory : DatabaseInserterFactory<SenateTerm>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter SenatorId = new() { Name = "senator_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };


    public override async Task<IDatabaseInserter<SenateTerm>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "senate_term",
            new DatabaseParameter[] {
                Id,
                SenatorId,
                SubdivisionId,
                DateRange
            }
        );
        return new SenateTermInserter(command);
    }
}
internal sealed class SenateTermInserter : DatabaseInserter<SenateTerm>
{

    internal SenateTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(SenateTerm senateTerm)
    {
        if (senateTerm.Id is null)
            throw new NullReferenceException();
        if (senateTerm.SenatorId is null)
            throw new NullReferenceException();
        Set(SenateTermInserterFactory.Id, senateTerm.Id.Value);
        Set(SenateTermInserterFactory.SenatorId, senateTerm.SenatorId.Value);
        Set(SenateTermInserterFactory.SubdivisionId, senateTerm.SubdivisionId);
        Set(SenateTermInserterFactory.DateRange, senateTerm.DateTimeRange);
        await _command.ExecuteNonQueryAsync();
    }
}
