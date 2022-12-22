using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
using System.Data;
namespace PoundPupLegacy.Db.Writers;

internal class SubdivisionWriter : DatabaseWriter<Subdivision>, IDatabaseWriter<Subdivision>
{
    public static DatabaseWriter<Subdivision> Create(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."subdivision" (id, name, country_id) VALUES(@id,@name,@country_id)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        command.Parameters.Add("name", NpgsqlDbType.Varchar);
        command.Parameters.Add("country_id", NpgsqlDbType.Integer);
        return new SubdivisionWriter(command);

    }

    internal SubdivisionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(Subdivision subdivision)
    {
        _command.Parameters["id"].Value = subdivision.Id;
        _command.Parameters["name"].Value = subdivision.Name;
        _command.Parameters["country_id"].Value = subdivision.CountryId;
        _command.ExecuteNonQuery();
    }
}
