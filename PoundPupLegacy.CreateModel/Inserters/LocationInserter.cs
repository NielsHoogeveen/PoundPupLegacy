using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class LocationInserterFactory : DatabaseInserterFactory<Location>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableStringDatabaseParameter Street = new() { Name = "street" };
    internal static NullableStringDatabaseParameter Additional = new() { Name = "additional" };
    internal static NullableStringDatabaseParameter City = new() { Name = "city" };
    internal static NullableStringDatabaseParameter PostalCode = new() { Name = "postal_code" };
    internal static NullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NullableDecimalDatabaseParameter Latitude = new() { Name = "latitude" };
    internal static NullableDecimalDatabaseParameter Longitude = new() { Name = "longitude" };
    public override async Task<IDatabaseInserter<Location>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var databaseParameters = new DatabaseParameter[] {
            Street,
            Additional,
            City,
            PostalCode,
            SubdivisionId,
            CountryId,
            Latitude,
            Longitude
            };

        var commandWithId = await CreateInsertStatementAsync(
            postgresConnection,
            "location",
            databaseParameters.ToImmutableList().Prepend(Id)
        );
        var commandWithoutId = await CreateAutoGenerateIdentityInsertStatementAsync(
            postgresConnection,
            "location",
            databaseParameters
        );
        return new LocationInserter(commandWithId, commandWithoutId);
    }
}
internal sealed class LocationInserter : DatabaseInserter<Location>
{
    private NpgsqlCommand _identityCommand;
    internal LocationInserter(NpgsqlCommand command, NpgsqlCommand identityCommand) : base(command)
    {
        _identityCommand = identityCommand;

    }

    public override async Task InsertAsync(Location location)
    {
        if (location.Id is null) {
            Set(LocationInserterFactory.Street, location.Street, _identityCommand);
            Set(LocationInserterFactory.Additional, location.Additional, _identityCommand);
            Set(LocationInserterFactory.City, location.City, _identityCommand);
            Set(LocationInserterFactory.PostalCode, location.PostalCode, _identityCommand);
            Set(LocationInserterFactory.SubdivisionId, location.SubdivisionId, _identityCommand);
            Set(LocationInserterFactory.CountryId, location.CountryId, _identityCommand);
            Set(LocationInserterFactory.Latitude, location.Latitude, _identityCommand);
            Set(LocationInserterFactory.Longitude, location.Longitude, _identityCommand);
            location.Id = await _command.ExecuteNonQueryAsync();
        }
        else {
            Set(LocationInserterFactory.Id,location.Id.Value);
            Set(LocationInserterFactory.Street, location.Street);
            Set(LocationInserterFactory.Additional, location.Additional);
            Set(LocationInserterFactory.City, location.City);
            Set(LocationInserterFactory.PostalCode, location.PostalCode);
            Set(LocationInserterFactory.SubdivisionId, location.SubdivisionId);
            Set(LocationInserterFactory.CountryId, location.CountryId);
            Set(LocationInserterFactory.Latitude, location.Latitude);
            Set(LocationInserterFactory.Longitude, location.Longitude);
            await _command.ExecuteNonQueryAsync();
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _identityCommand.DisposeAsync();
    }

}
