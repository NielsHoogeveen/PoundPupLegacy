using PoundPupLegacy.EditModel.Readers;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class OrganizationSearchService : SearchService<PartyItem.OrganizationItem.OrganizationListItem, OrganizationsReaderRequest>
{
    public OrganizationSearchService(
        IDbConnection connection,
        IEnumerableDatabaseReaderFactory<OrganizationsReaderRequest, PartyItem.OrganizationItem.OrganizationListItem> organizationsReaderFactory): base(connection, organizationsReaderFactory)
    {
    }
    protected override OrganizationsReaderRequest GetRequest(int tenantId, string searchString)
    {
        return new OrganizationsReaderRequest {
            TenantId = tenantId,
            SearchString = searchString

        };
    }
}
