using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
using System.Data;
namespace PoundPupLegacy.Db.Writers;

internal class BoundCountryWriter : DatabaseWriter<BoundCountry>, IDatabaseWriter<BoundCountry>
{
    public static DatabaseWriter<BoundCountry> Create(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."bound_country" (id, binding_country_id) VALUES(@id,@binding_country_id)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        command.Parameters.Add("binding_country_id", NpgsqlDbType.Integer);
        return new BoundCountryWriter(command);
    }
    private BoundCountryWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(BoundCountry country)
    {
        _command.Parameters["id"].Value = country.Id;
        _command.Parameters["binding_country_id"].Value = country.BindingCountryId;
        _command.ExecuteNonQuery();
    }
}
