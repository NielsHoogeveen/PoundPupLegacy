using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class LocationLocatableWriter : DatabaseWriter<LocationLocatable>, IDatabaseWriter<LocationLocatable>
{
    private const string LOCATION_ID = "location_id";
    private const string LOCATABLE_ID = "locatable_id";
    public static DatabaseWriter<LocationLocatable> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
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

    internal override void Write(LocationLocatable locationLocatable)
    {
        WriteValue(locationLocatable.LocationId, LOCATION_ID);
        WriteValue(locationLocatable.LocatableId, LOCATABLE_ID);
        _command.ExecuteNonQuery();
    }
}
