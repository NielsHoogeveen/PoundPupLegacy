namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class LocationLocatableInserter : DatabaseInserter<LocationLocatable>, IDatabaseInserter<LocationLocatable>
{
    private const string LOCATION_ID = "location_id";
    private const string LOCATABLE_ID = "locatable_id";
    public static async Task<DatabaseInserter<LocationLocatable>> CreateAsync(NpgsqlConnection connection)
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

        return new LocationLocatableInserter(command);

    }

    internal LocationLocatableInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(LocationLocatable locationLocatable)
    {
        WriteValue(locationLocatable.LocationId, LOCATION_ID);
        WriteValue(locationLocatable.LocatableId, LOCATABLE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
