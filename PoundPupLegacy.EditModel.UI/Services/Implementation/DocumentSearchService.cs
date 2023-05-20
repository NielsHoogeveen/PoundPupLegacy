using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DocumentSearchService(
    IDbConnection connection,
    ILogger<DocumentSearchService> logger,
    IEnumerableDatabaseReaderFactory<DocumentsReaderRequest, DocumentListItem> documentsReaderFactory
) : SearchService<DocumentListItem, DocumentsReaderRequest>(
    connection, 
    logger, 
    documentsReaderFactory
)
{
    protected override DocumentsReaderRequest GetRequest(int tenantId, string searchString)
    {
        return new DocumentsReaderRequest {
            TenantId = tenantId,
            SearchString = searchString
        };
    }
}
