namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class LocationLocatableInserterFactory : DatabaseInserterFactory<LocationLocatable>
{
    internal static NonNullableIntegerDatabaseParameter LocationId = new() { Name = "location_id" };
    internal static NonNullableIntegerDatabaseParameter LocatableId = new() { Name = "locatable_id" };

    public override async Task<IDatabaseInserter<LocationLocatable>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "location_locatable",
            new DatabaseParameter[] {
                LocationId,
                LocatableId
            }
        );
        return new LocationLocatableInserter(command);
    }
}
internal sealed class LocationLocatableInserter : DatabaseInserter<LocationLocatable>
{
    internal LocationLocatableInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(LocationLocatable locationLocatable)
    {
        if(locationLocatable.LocationId == null)
            throw new NullReferenceException();
        Set(LocationLocatableInserterFactory.LocationId, locationLocatable.LocationId.Value);
        Set(LocationLocatableInserterFactory.LocatableId, locationLocatable.LocatableId);
        await _command.ExecuteNonQueryAsync();
    }
}
