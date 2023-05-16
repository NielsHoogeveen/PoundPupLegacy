namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class OrganizationSearchService : SearchService<OrganizationListItem, OrganizationsReaderRequest>
{
    public OrganizationSearchService(
        IDbConnection connection,
        IEnumerableDatabaseReaderFactory<OrganizationsReaderRequest, OrganizationListItem> organizationsReaderFactory) : base(connection, organizationsReaderFactory)
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
