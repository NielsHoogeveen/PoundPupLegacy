using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Services;

public interface IEditorService
{
    Task<Document?> GetDocument(int urlId, int userId, int tenantId);
    Task<BlogPost?> GetBlogPost(int urlId, int userId, int tenantId);
    Task<Article?> GetArticle(int urlId, int userId, int tenantId);
    Task<Discussion?> GetDiscussion(int urlId, int userId, int tenantId);
    Task<Organization?> GetOrganization(int urlId, int userId, int tenantId);
    Task<BlogPost?> GetNewBlogPost(int userId, int tenantId);
    Task<Article?> GetNewArticle(int userId, int tenantId);
    Task<Discussion?> GetNewDiscussion(int userId, int tenantId);
    Task Save(SimpleTextNode simpleTextNode);
    Task Save(Document document);
    Task Save(Organization organization);
}
