using Npgsql;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class SeconcLevelGlobalRegionWriter : DatabaseWriter<SecondLevelGlobalRegion>
{

    public SeconcLevelGlobalRegionWriter(NpgsqlCommand command) : base(command)
    {
    }

    public override void Write(SecondLevelGlobalRegion node)
    {
        _command.Parameters["id"].Value = node.Id;
        _command.Parameters["first_level_global_region_id"].Value = node.FirstLevelGlobalRegionId;

        _command.ExecuteNonQuery();
    }
}
