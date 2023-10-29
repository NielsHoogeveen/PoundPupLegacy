using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class CreateOptionsService(
    NpgsqlDataSource dataSource,
    ILogger<AuthenticationService> logger,
    ISingleItemDatabaseReaderFactory<CreateOptionsReaderRequest, List<CreateOption>> readerFactory
) : DatabaseService(dataSource, logger), ICreateOptionsService
{
    public async Task<List<CreateOption>> GetCreateOptions(int tenantId, int userId)
    {
        return await WithConnection(async (connection) => {
            var reader = await readerFactory.CreateAsync(connection);
            var result =  await reader.ReadAsync(new CreateOptionsReaderRequest {
                TenantId = tenantId,
                UserId = userId
            });
            if(result is null) {
                return new List<CreateOption>();
            }
            return result;
        });
    }
}
