using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class SubgroupsService(
    IDbConnection connection,
    ILogger<AuthenticationService> logger,
    ISingleItemDatabaseReaderFactory<SubgroupsReaderRequest, List<Subgroup>> readerFactory

) : DatabaseService(connection, logger), ISubgroupsService
{
    public async Task<List<Subgroup>> GetSubgroups(int tenantId)
    {
        return await WithConnection(async (connection) => {
            var reader = await readerFactory.CreateAsync(connection);
            var result =  await reader.ReadAsync(new SubgroupsReaderRequest {
                TenantId = tenantId
            });
            if(result is null) {
                return new List<Subgroup>();
            }
            return result;
        });
    }
}
