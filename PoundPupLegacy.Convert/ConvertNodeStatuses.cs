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

    private static async Task MigrateNodeStatuses(NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await NodeStatusCreator.CreateAsync(GetNodeStatuses().ToAsyncEnumerable(), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}
