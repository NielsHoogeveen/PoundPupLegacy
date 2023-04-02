using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Services;

public interface IEditorService
{
    Task<Document?> GetDocument(int urlId, int userId, int tenantId);
    Task Save(Document document);
}
