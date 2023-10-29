using Microsoft.Extensions.Logging;
using Npgsql;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class GeographicalEntitySearchService(
    NpgsqlDataSource dataSource,
    ILogger<GeographicalEntitySearchService> logger,
    IEnumerableDatabaseReaderFactory<GeographicalEntitiesReaderRequest, GeographicalEntityListItem> readerFactory
) : SearchService<GeographicalEntityListItem, GeographicalEntitiesReaderRequest>(
    dataSource, 
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
