namespace PoundPupLegacy.EditModel.UI.Services;

public interface IDocumentableDocumentsSearchService
{
    [RequireNamedArgs]
    Task<List<DocumentableDocument>> GetDocumentableDocuments(int nodeId, int userId, int tenantId, string searchString);
}
