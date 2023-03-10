using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Services;

public interface IEditorService
{
    Task<Document?> GetDocument(int id);
    Task<BlogPost?> GetBlogPost(int id);
    Task<Article?> GetArticle(int id);
    Task<Discussion?> GetDiscussion(int id);
    Task<Organization?> GetOrganization(int id);
    Task<BlogPost?> GetNewBlogPost();
    Task<Article?> GetNewArticle();
    Task<Discussion?> GetNewDiscussion();
    Task Save(SimpleTextNode simpleTextNode);
    Task Save(Document document);
    Task Save(Organization organization);
}
