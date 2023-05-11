using PoundPupLegacy.EditModel.Readers;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class PartySearchService : SearchService<PartyListItem, PartiesReaderRequest>
{
    public PartySearchService(
        IDbConnection connection,
        IEnumerableDatabaseReaderFactory<PartiesReaderRequest, PartyListItem> readerFactory): base(connection, readerFactory)
    {
    }
    protected override PartiesReaderRequest GetRequest(int tenantId, string searchString)
    {
        return new PartiesReaderRequest {
            TenantId = tenantId,
            SearchString = searchString

        };
    }
}
