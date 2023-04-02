namespace PoundPupLegacy.CreateModel.Inserters;

public class SecondLevelGlobalRegionInserter : DatabaseInserter<SecondLevelGlobalRegion>, IDatabaseInserter<SecondLevelGlobalRegion>
{
    private const string ID = "id";
    private const string FIRST_LEVEL_GLOBAL_REGION_ID = "first_level_global_region_id";
    public static async Task<DatabaseInserter<SecondLevelGlobalRegion>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "second_level_global_region",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = FIRST_LEVEL_GLOBAL_REGION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new SecondLevelGlobalRegionInserter(command);
    }

    public SecondLevelGlobalRegionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(SecondLevelGlobalRegion region)
    {
        if (region.Id is null)
            throw new NullReferenceException();

        WriteValue(region.Id, ID);
        WriteValue(region.FirstLevelGlobalRegionId, FIRST_LEVEL_GLOBAL_REGION_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
