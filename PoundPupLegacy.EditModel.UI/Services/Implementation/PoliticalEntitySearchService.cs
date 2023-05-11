using PoundPupLegacy.EditModel.Readers;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class PoliticalEntitySearchService : SearchService<PoliticalEntityListItem, PoliticalEntitiesReaderRequest>
{
    public PoliticalEntitySearchService(
        IDbConnection connection,
        IEnumerableDatabaseReaderFactory<PoliticalEntitiesReaderRequest, PoliticalEntityListItem> readerFactory) : base(connection, readerFactory)
    {
    }
    protected override PoliticalEntitiesReaderRequest GetRequest(int tenantId, string searchString)
    {
        return new PoliticalEntitiesReaderRequest {
            TenantId = tenantId,
            SearchString = searchString

        };
    }
}
