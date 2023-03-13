using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Services;

public interface IEditorService
{
    Task<IEnumerable<SubdivisionListItem>> GetSubdivisions(int countryId);
    Task<Document?> GetDocument(int id, int userId, int tenantId);
    Task<BlogPost?> GetBlogPost(int id, int userId, int tenantId);
    Task<Article?> GetArticle(int id, int userId, int tenantId);
    Task<Discussion?> GetDiscussion(int id, int userId, int tenantId);
    Task<Organization?> GetOrganization(int id, int userId, int tenantId);
    Task<BlogPost?> GetNewBlogPost(int userId, int tenantId);
    Task<Article?> GetNewArticle(int userId, int tenantId);
    Task<Discussion?> GetNewDiscussion(int userId, int tenantId);
    Task Save(SimpleTextNode simpleTextNode);
    Task Save(Document document);
    Task Save(Organization organization);
}
