using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class ListOptionsService(
    NpgsqlDataSource dataSource,
    ILogger<AuthenticationService> logger,
    ISingleItemDatabaseReaderFactory<ListOptionsReaderRequest, List<ListOption>> readerFactory
) : DatabaseService(dataSource, logger), IListOptionsService
{
    public async Task<List<ListOption>> GetListOptions(int tenantId, int userId)
    {
        return await WithConnection(async (connection) => {
            var reader = await readerFactory.CreateAsync(connection);
            var result =  await reader.ReadAsync(new ListOptionsReaderRequest {
                TenantId = tenantId,
                UserId = userId
            });
            if(result is null) {
                return new List<ListOption>();
            }
            return result;
        });
    }
}
