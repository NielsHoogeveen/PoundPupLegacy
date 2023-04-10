namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class BasicSecondLevelSubdivisionInserterFactory : DatabaseInserterFactory<BasicSecondLevelSubdivision>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter IntermediateLevelSubdivisionId = new() { Name = "intermediate_level_subdivision_id" };

    public override async Task<IDatabaseInserter<BasicSecondLevelSubdivision>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "basic_second_level_subdivision",
            new DatabaseParameter[] {
                Id,
                IntermediateLevelSubdivisionId
            }
        );
        return new BasicSecondLevelSubdivisionInserter(command);
    }

}
internal sealed class BasicSecondLevelSubdivisionInserter : DatabaseInserter<BasicSecondLevelSubdivision>
{

    internal BasicSecondLevelSubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(BasicSecondLevelSubdivision country)
    {
        if (country.Id is null)
            throw new NullReferenceException();
        Set(BasicSecondLevelSubdivisionInserterFactory.Id, country.Id.Value);
        Set(BasicSecondLevelSubdivisionInserterFactory.IntermediateLevelSubdivisionId, country.IntermediateLevelSubdivisionId);
        await _command.ExecuteNonQueryAsync();
    }
}
