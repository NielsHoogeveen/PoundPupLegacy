using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Services;

public interface IDocumentableDocumentsSearchService
{
    Task<List<DocumentableDocument>> GetDocumentableDocuments(int nodeId, string str);
}
