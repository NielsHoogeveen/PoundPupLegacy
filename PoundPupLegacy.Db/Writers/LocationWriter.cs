using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class LocationWriter : DatabaseWriter<Location>, IDatabaseWriter<Location>
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
    public static DatabaseWriter<Location> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "location",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
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

            }
        );

        return new LocationWriter(command);

    }

    internal LocationWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(Location location)
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
        _command.ExecuteNonQuery();
    }
}
