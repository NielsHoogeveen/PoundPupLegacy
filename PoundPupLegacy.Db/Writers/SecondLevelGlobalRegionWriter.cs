namespace PoundPupLegacy.Db.Writers;

public class SecondLevelGlobalRegionWriter : DatabaseWriter<SecondLevelGlobalRegion>, IDatabaseWriter<SecondLevelGlobalRegion>
{
    private const string ID = "id";
    private const string FIRST_LEVEL_GLOBAL_REGION_ID = "first_level_global_region_id";
    public static DatabaseWriter<SecondLevelGlobalRegion> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
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
        return new SecondLevelGlobalRegionWriter(command);
    }

    public SecondLevelGlobalRegionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(SecondLevelGlobalRegion region)
    {
        if (region.Id is null)
            throw new NullReferenceException();

        WriteValue(region.Id, ID);
        WriteValue(region.FirstLevelGlobalRegionId, FIRST_LEVEL_GLOBAL_REGION_ID);
        _command.ExecuteNonQuery();
    }
}
