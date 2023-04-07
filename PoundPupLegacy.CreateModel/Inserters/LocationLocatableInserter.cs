namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class LocationLocatableInserterFactory : DatabaseInserterFactory<LocationLocatable>
{
    public override async Task<IDatabaseInserter<LocationLocatable>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "location_locatable",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = LocationLocatableInserter.LOCATION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = LocationLocatableInserter.LOCATABLE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new LocationLocatableInserter(command);
    }
}
internal sealed class LocationLocatableInserter : DatabaseInserter<LocationLocatable>
{
    internal const string LOCATION_ID = "location_id";
    internal const string LOCATABLE_ID = "locatable_id";

    internal LocationLocatableInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(LocationLocatable locationLocatable)
    {
        SetParameter(locationLocatable.LocationId, LOCATION_ID);
        SetParameter(locationLocatable.LocatableId, LOCATABLE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
