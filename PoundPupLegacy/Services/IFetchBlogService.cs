using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface IFetchBlogService
{
    Task<Blog> FetchBlog(HttpContext context, int accessRoleId, int startIndex, int length);
}

