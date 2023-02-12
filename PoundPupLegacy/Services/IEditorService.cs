using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Services;

public interface IEditorService
{
    Task<BlogPost?> GetBlogPost(int id);

    Task Save(BlogPost post);
}
