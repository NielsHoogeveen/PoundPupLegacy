using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Services;

public interface IEditorService
{
    Task<Document?> GetDocument(int id);
    Task<BlogPost?> GetBlogPost(int id);
    Task<BlogPost?> GetNewBlogPost();
    Task Save(BlogPost post);
    Task Save(Document document);
}
