using System.Collections.Immutable;

namespace PoundPupLegacy.Db.Writers;

internal sealed class LocationWriter : DatabaseWriter<Location>, IDatabaseWriter<Location>
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
    public static async Task<DatabaseWriter<Location>> CreateAsync(NpgsqlConnection connection)
    {
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
            connection,
            "location",
            columnDefitions.ToImmutableList().Prepend(new ColumnDefinition
            {
                Name = ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            })
        );
        var commandWithoutId = await CreateIdentityInsertStatementAsync(
            connection,
            "location",
            columnDefitions
        );

        return new LocationWriter(commandWithId, commandWithoutId);

    }

    private NpgsqlCommand _identityCommand;
    internal LocationWriter(NpgsqlCommand command, NpgsqlCommand identityCommand) : base(command)
    {
        _identityCommand = identityCommand;

    }

    internal override async Task WriteAsync(Location location)
    {
        if (location.Id is null)
        {
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
        else
        {
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
