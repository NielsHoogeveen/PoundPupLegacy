using PoundPupLegacy.Common;
using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Services;

public interface IDocumentableDocumentsSearchService
{
    [RequireNamedArgs]
    Task<List<DocumentableDocument>> GetDocumentableDocuments(int nodeId, int userId, int tenantId, string searchString);
}
