using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
using System.Data;
namespace PoundPupLegacy.Db.Writers;

public class SecondLevelGlobalRegionWriter : DatabaseWriter<SecondLevelGlobalRegion>, IDatabaseWriter<SecondLevelGlobalRegion>
{
    public static DatabaseWriter<SecondLevelGlobalRegion> Create(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."second_level_global_region" (id, first_level_global_region_id) VALUES(@id,@first_level_global_region_id)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        command.Parameters.Add("first_level_global_region_id", NpgsqlDbType.Integer);
        return new SecondLevelGlobalRegionWriter(command);
    }

    public SecondLevelGlobalRegionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(SecondLevelGlobalRegion node)
    {
        _command.Parameters["id"].Value = node.Id;
        _command.Parameters["first_level_global_region_id"].Value = node.FirstLevelGlobalRegionId;

        _command.ExecuteNonQuery();
    }
}
