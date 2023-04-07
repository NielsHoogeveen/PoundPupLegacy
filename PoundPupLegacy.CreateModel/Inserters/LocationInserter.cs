using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class LocationInserterFactory : DatabaseInserterFactory<Location>
{
    public override async Task<IDatabaseInserter<Location>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var columnDefitions = new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = LocationInserter.STREET,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = LocationInserter.ADDITIONAL,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },

                new ColumnDefinition{
                    Name = LocationInserter.CITY,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = LocationInserter.POSTAL_CODE,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = LocationInserter.SUBDIVIONS_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = LocationInserter.COUNTRY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = LocationInserter.LATITUDE,
                    NpgsqlDbType = NpgsqlDbType.Numeric
                },
                new ColumnDefinition{
                    Name = LocationInserter.LONGITUDE,
                    NpgsqlDbType = NpgsqlDbType.Numeric
                },

            };

        var commandWithId = await CreateInsertStatementAsync(
            postgresConnection,
            "location",
            columnDefitions.ToImmutableList().Prepend(new ColumnDefinition {
                Name = LocationInserter.ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            })
        );
        var commandWithoutId = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "location",
            columnDefitions
        );

        return new LocationInserter(commandWithId, commandWithoutId);

    }

}
internal sealed class LocationInserter : DatabaseInserter<Location>
{
    internal const string ID = "id";
    internal const string STREET = "street";
    internal const string ADDITIONAL = "additional";
    internal const string CITY = "city";
    internal const string POSTAL_CODE = "postal_code";
    internal const string SUBDIVIONS_ID = "subdivision_id";
    internal const string COUNTRY_ID = "country_id";
    internal const string LATITUDE = "latitude";
    internal const string LONGITUDE = "longitude";

    private NpgsqlCommand _identityCommand;
    internal LocationInserter(NpgsqlCommand command, NpgsqlCommand identityCommand) : base(command)
    {
        _identityCommand = identityCommand;

    }

    public override async Task InsertAsync(Location location)
    {
        if (location.Id is null) {
            SetNullableParameter(location.Street, STREET, _identityCommand);
            SetNullableParameter(location.Additional, ADDITIONAL, _identityCommand);
            SetNullableParameter(location.City, CITY, _identityCommand);
            SetNullableParameter(location.PostalCode, POSTAL_CODE, _identityCommand);
            SetNullableParameter(location.SubdivisionId, SUBDIVIONS_ID, _identityCommand);
            SetParameter(location.CountryId, COUNTRY_ID, _identityCommand);
            SetNullableParameter(location.Latitude, LATITUDE, _identityCommand);
            SetNullableParameter(location.Longitude, LONGITUDE, _identityCommand);
            location.Id = await _command.ExecuteNonQueryAsync();
        }
        else {
            SetParameter(location.Id, ID);
            SetNullableParameter(location.Street, STREET);
            SetNullableParameter(location.Additional, ADDITIONAL);
            SetNullableParameter(location.City, CITY);
            SetNullableParameter(location.PostalCode, POSTAL_CODE);
            SetNullableParameter(location.SubdivisionId, SUBDIVIONS_ID);
            SetParameter(location.CountryId, COUNTRY_ID);
            SetNullableParameter(location.Latitude, LATITUDE);
            SetNullableParameter(location.Longitude, LONGITUDE);
            await _command.ExecuteNonQueryAsync();
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _identityCommand.DisposeAsync();
    }

}
