namespace PoundPupLegacy.CreateModel.Inserters;
public class SecondLevelGlobalRegionInserterFactory : DatabaseInserterFactory<SecondLevelGlobalRegion>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter FirstLevelGlobalRegionId = new() { Name = "first_level_global_region_id" };

    public override async Task<IDatabaseInserter<SecondLevelGlobalRegion>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "second_level_global_region",
            new DatabaseParameter[] {
                Id,
                FirstLevelGlobalRegionId
            }
        );
        return new SecondLevelGlobalRegionInserter(command);
    }
}
public class SecondLevelGlobalRegionInserter : DatabaseInserter<SecondLevelGlobalRegion>
{

    public SecondLevelGlobalRegionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(SecondLevelGlobalRegion region)
    {
        if (region.Id is null)
            throw new NullReferenceException();

        Set(SecondLevelGlobalRegionInserterFactory.Id, region.Id.Value);
        Set(SecondLevelGlobalRegionInserterFactory.FirstLevelGlobalRegionId, region.FirstLevelGlobalRegionId);
        await _command.ExecuteNonQueryAsync();
    }
}
