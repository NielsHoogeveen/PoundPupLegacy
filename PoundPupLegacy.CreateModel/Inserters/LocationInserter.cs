using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class LocationInserter : DatabaseInserter<Location>, IDatabaseInserter<Location>
{
    private const string ID = "id";
    private const string STREET = "street";
    private const string ADDITIONAL = "additional";
    private const string CITY = "city";
    private const string POSTAL_CODE = "postal_code";
    private const string SUBDIVIONS_ID = "subdivision_id";
    private const string COUNTRY_ID = "country_id";
    private const string LATITUDE = "latitude";
    private const string LONGITUDE = "longitude";
    public static async Task<DatabaseInserter<Location>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var columnDefitions = new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = STREET,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = ADDITIONAL,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },

                new ColumnDefinition{
                    Name = CITY,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = POSTAL_CODE,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = SUBDIVIONS_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = COUNTRY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = LATITUDE,
                    NpgsqlDbType = NpgsqlDbType.Numeric
                },
                new ColumnDefinition{
                    Name = LONGITUDE,
                    NpgsqlDbType = NpgsqlDbType.Numeric
                },

            };

        var commandWithId = await CreateInsertStatementAsync(
            postgresConnection,
            "location",
            columnDefitions.ToImmutableList().Prepend(new ColumnDefinition {
                Name = ID,
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

    private NpgsqlCommand _identityCommand;
    internal LocationInserter(NpgsqlCommand command, NpgsqlCommand identityCommand) : base(command)
    {
        _identityCommand = identityCommand;

    }

    public override async Task InsertAsync(Location location)
    {
        if (location.Id is null) {
            WriteNullableValue(location.Street, STREET, _identityCommand);
            WriteNullableValue(location.Additional, ADDITIONAL, _identityCommand);
            WriteNullableValue(location.City, CITY, _identityCommand);
            WriteNullableValue(location.PostalCode, POSTAL_CODE, _identityCommand);
            WriteNullableValue(location.SubdivisionId, SUBDIVIONS_ID, _identityCommand);
            WriteValue(location.CountryId, COUNTRY_ID, _identityCommand);
            WriteNullableValue(location.Latitude, LATITUDE, _identityCommand);
            WriteNullableValue(location.Longitude, LONGITUDE, _identityCommand);
            location.Id = await _command.ExecuteNonQueryAsync();
        }
        else {
            WriteValue(location.Id, ID);
            WriteNullableValue(location.Street, STREET);
            WriteNullableValue(location.Additional, ADDITIONAL);
            WriteNullableValue(location.City, CITY);
            WriteNullableValue(location.PostalCode, POSTAL_CODE);
            WriteNullableValue(location.SubdivisionId, SUBDIVIONS_ID);
            WriteValue(location.CountryId, COUNTRY_ID);
            WriteNullableValue(location.Latitude, LATITUDE);
            WriteNullableValue(location.Longitude, LONGITUDE);
            await _command.ExecuteNonQueryAsync();
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _identityCommand.DisposeAsync();
    }

}
