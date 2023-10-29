using Microsoft.Extensions.Logging;
using Npgsql;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class PoliticalEntitySearchService(
    NpgsqlDataSource dataSource,
    ILogger<PoliticalEntitySearchService> logger,
    IEnumerableDatabaseReaderFactory<PoliticalEntitiesReaderRequest, PoliticalEntityListItem> readerFactory
) : SearchService<PoliticalEntityListItem, PoliticalEntitiesReaderRequest>(
    dataSource, 
    logger, 
    readerFactory
)
{
    protected override PoliticalEntitiesReaderRequest GetRequest(int tenantId, string searchString)
    {
        return new PoliticalEntitiesReaderRequest {
            TenantId = tenantId,
            SearchString = searchString
        };
    }
}
