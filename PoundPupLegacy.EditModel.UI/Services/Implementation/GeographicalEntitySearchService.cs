namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class GeographicalEntitySearchService : SearchService<GeographicalEntityListItem, GeographicalEntitiesReaderRequest>
{
    public GeographicalEntitySearchService(
        IDbConnection connection,
        IEnumerableDatabaseReaderFactory<GeographicalEntitiesReaderRequest, GeographicalEntityListItem> readerFactory) : base(connection, readerFactory)
    {
    }
    protected override GeographicalEntitiesReaderRequest GetRequest(int tenantId, string searchString)
    {
        return new GeographicalEntitiesReaderRequest {
            TenantId = tenantId,
            SearchString = searchString

        };
    }
}
