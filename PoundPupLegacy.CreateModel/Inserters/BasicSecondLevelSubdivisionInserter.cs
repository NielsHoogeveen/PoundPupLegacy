namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class BasicSecondLevelSubdivisionInserterFactory : DatabaseInserterFactory<BasicSecondLevelSubdivision>
{
    public override async Task<IDatabaseInserter<BasicSecondLevelSubdivision>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "basic_second_level_subdivision",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = BasicSecondLevelSubdivisionInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = BasicSecondLevelSubdivisionInserter.INTERMEDIATE_LEVEL_SUBDIVISION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new BasicSecondLevelSubdivisionInserter(command);
    }

}
internal sealed class BasicSecondLevelSubdivisionInserter : DatabaseInserter<BasicSecondLevelSubdivision>
{
    internal const string ID = "id";
    internal const string INTERMEDIATE_LEVEL_SUBDIVISION_ID = "intermediate_level_subdivision_id";

    internal BasicSecondLevelSubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(BasicSecondLevelSubdivision country)
    {
        if (country.Id is null)
            throw new NullReferenceException();
        SetParameter(country.Id, ID);
        SetParameter(country.IntermediateLevelSubdivisionId, INTERMEDIATE_LEVEL_SUBDIVISION_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
