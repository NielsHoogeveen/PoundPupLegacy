using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class BoundCountryWriter : DatabaseWriter<BoundCountry>, IDatabaseWriter<BoundCountry>
{
    private const string ID = "id";
    private const string BINDING_COUNTRY_ID = "binding_country_id";
    public static DatabaseWriter<BoundCountry> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "bound_country",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = BINDING_COUNTRY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new BoundCountryWriter(command);
    }
    private BoundCountryWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(BoundCountry country)
    {
        WriteValue(country.Id, ID);
        WriteValue(country.BindingCountryId, BINDING_COUNTRY_ID);
        _command.ExecuteNonQuery();
    }
}
