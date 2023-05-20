using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class GeographicalEntitySearchService(
    IDbConnection connection,
    ILogger<GeographicalEntitySearchService> logger,
    IEnumerableDatabaseReaderFactory<GeographicalEntitiesReaderRequest, GeographicalEntityListItem> readerFactory
) : SearchService<GeographicalEntityListItem, GeographicalEntitiesReaderRequest>(
    connection, 
    logger, 
    readerFactory
)
{
    protected override GeographicalEntitiesReaderRequest GetRequest(int tenantId, string searchString)
    {
        return new GeographicalEntitiesReaderRequest {
            TenantId = tenantId,
            SearchString = searchString

        };
    }
}
