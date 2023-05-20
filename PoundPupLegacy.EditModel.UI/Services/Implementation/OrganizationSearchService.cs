using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class OrganizationSearchService(
    IDbConnection connection,
    ILogger<OrganizationSearchService> logger,
    IEnumerableDatabaseReaderFactory<OrganizationsReaderRequest, OrganizationListItem> organizationsReaderFactory
) : SearchService<OrganizationListItem, OrganizationsReaderRequest>(
    connection, 
    logger, 
    organizationsReaderFactory
)
{
    protected override OrganizationsReaderRequest GetRequest(int tenantId, string searchString)
    {
        return new OrganizationsReaderRequest {
            TenantId = tenantId,
            SearchString = searchString
        };
    }
}
