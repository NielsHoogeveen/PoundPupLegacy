using Microsoft.Extensions.Logging;
using Npgsql;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class UnitedStatesCountySearchService(
    NpgsqlDataSource dataSource,
    ILogger<UnitedStatesCountySearchService> logger,
    IEnumerableDatabaseReaderFactory<UnitedStatesCountiesReaderRequest, UnitedStatesCountyListItem> readerFactory
) : SearchService<UnitedStatesCountyListItem, UnitedStatesCountiesReaderRequest>(
    dataSource, 
    logger, 
    readerFactory
)
{
    protected override UnitedStatesCountiesReaderRequest GetRequest(int tenantId, string searchString)
    {
        return new UnitedStatesCountiesReaderRequest {
            TenantId = tenantId,
            SearchString = searchString

        };
    }
}
