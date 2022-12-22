using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
using System.Data;
namespace PoundPupLegacy.Db.Writers;

internal class ISOCodedSubdivisionWriter : DatabaseWriter<ISOCodedSubdivision>, IDatabaseWriter<ISOCodedSubdivision>
{
    public static DatabaseWriter<ISOCodedSubdivision> Create(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."iso_coded_subdivision" (id, iso_3166_2_code) VALUES(@id,@iso_3166_2_code)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        command.Parameters.Add("iso_3166_2_code", NpgsqlDbType.Char);
        return new ISOCodedSubdivisionWriter(command);

    }
    private ISOCodedSubdivisionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(ISOCodedSubdivision country)
    {
        _command.Parameters["id"].Value = country.Id;
        _command.Parameters["iso_3166_2_code"].Value = country.ISO3166_2_Code;
        _command.ExecuteNonQuery();
    }
}
