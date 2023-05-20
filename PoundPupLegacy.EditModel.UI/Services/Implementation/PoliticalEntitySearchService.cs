using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class PoliticalEntitySearchService(
    IDbConnection connection,
    ILogger<PoliticalEntitySearchService> logger,
    IEnumerableDatabaseReaderFactory<PoliticalEntitiesReaderRequest, PoliticalEntityListItem> readerFactory
) : SearchService<PoliticalEntityListItem, PoliticalEntitiesReaderRequest>(
    connection, 
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
