using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal partial class Program
{

    private static IEnumerable<PublicationStatus> GetNodeStatuses()
    {
        return new List<PublicationStatus>
        {
            new PublicationStatus
            {
                Id = 0,
                Name = "Not Published",
            },
            new PublicationStatus
            {
                Id = 1,
                Name = "Published publically",
            },
            new PublicationStatus
            {
                Id = 2,
                Name = "Published privately",
            },
        };
    }

    private static async Task MigratePublicationStatuses(NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await PublicationStatusCreator.CreateAsync(GetNodeStatuses().ToAsyncEnumerable(), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}
