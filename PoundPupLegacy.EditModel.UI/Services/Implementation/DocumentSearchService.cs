using Microsoft.Extensions.Logging;
using Npgsql;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DocumentSearchService(
    NpgsqlDataSource dataSource,
    ILogger<DocumentSearchService> logger,
    IEnumerableDatabaseReaderFactory<DocumentsReaderRequest, DocumentListItem> documentsReaderFactory
) : SearchService<DocumentListItem, DocumentsReaderRequest>(
    dataSource, 
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
