using PoundPupLegacy.EditModel.Readers;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DocumentSearchService : SearchService<DocumentListItem, DocumentsReaderRequest>
{
    public DocumentSearchService(
        IDbConnection connection,
        IEnumerableDatabaseReaderFactory<DocumentsReaderRequest, DocumentListItem> documentsReaderFactory) : base(connection, documentsReaderFactory)
    {
    }
    protected override DocumentsReaderRequest GetRequest(int tenantId, string searchString)
    {
        return new DocumentsReaderRequest {
            TenantId = tenantId,
            SearchString = searchString

        };
    }
}
