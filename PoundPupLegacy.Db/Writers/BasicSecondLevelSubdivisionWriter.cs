using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
using System.Data;
namespace PoundPupLegacy.Db.Writers;

internal class BasicSecondLevelSubdivisionWriter : DatabaseWriter<BasicSecondLevelSubdivision>, IDatabaseWriter<BasicSecondLevelSubdivision>
{
    public static DatabaseWriter<BasicSecondLevelSubdivision> Create(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."basic_second_level_subdivision" (id, intermediate_level_subdivision_id) VALUES(@id,@intermediate_level_subdivision_id)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        command.Parameters.Add("intermediate_level_subdivision_id", NpgsqlDbType.Integer);
        return new BasicSecondLevelSubdivisionWriter(command);
    }
    private BasicSecondLevelSubdivisionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(BasicSecondLevelSubdivision country)
    {
        _command.Parameters["id"].Value = country.Id;
        _command.Parameters["intermediate_level_subdivision_id"].Value = country.IntermediateLevelSubdivisionId;
        _command.ExecuteNonQuery();
    }
}
