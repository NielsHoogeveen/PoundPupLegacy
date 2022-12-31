using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal partial class Program
{

    private static IEnumerable<NodeStatus> GetNodeStatuses()
    {
        return new List<NodeStatus>
        {
            new NodeStatus
            {
                Id = 0,
                Name = "Not Published",
            },
            new NodeStatus
            {
                Id = 1,
                Name = "Published",
            },

        };
    }

    private static void MigrateNodeStatuses(NpgsqlConnection connection)
    {
        NodeStatusCreator.Create(GetNodeStatuses(), connection);
    }

}
