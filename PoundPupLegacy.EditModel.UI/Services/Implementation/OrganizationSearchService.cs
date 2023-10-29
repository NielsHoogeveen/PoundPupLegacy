using Microsoft.Extensions.Logging;
using Npgsql;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class OrganizationSearchService(
    NpgsqlDataSource dataSource,
    ILogger<OrganizationSearchService> logger,
    IEnumerableDatabaseReaderFactory<OrganizationsReaderRequest, OrganizationListItem> organizationsReaderFactory
) : SearchService<OrganizationListItem, OrganizationsReaderRequest>(
    dataSource, 
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
