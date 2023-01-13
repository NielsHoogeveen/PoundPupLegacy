namespace PoundPupLegacy.Db.Writers;

internal sealed class LocationLocatableWriter : DatabaseWriter<LocationLocatable>, IDatabaseWriter<LocationLocatable>
{
    private const string LOCATION_ID = "location_id";
    private const string LOCATABLE_ID = "locatable_id";
    public static async Task<DatabaseWriter<LocationLocatable>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "location_locatable",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = LOCATION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = LOCATABLE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );

        return new LocationLocatableWriter(command);

    }

    internal LocationLocatableWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(LocationLocatable locationLocatable)
    {
        WriteValue(locationLocatable.LocationId, LOCATION_ID);
        WriteValue(locationLocatable.LocatableId, LOCATABLE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
