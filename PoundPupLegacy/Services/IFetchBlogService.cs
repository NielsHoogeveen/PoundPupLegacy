using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface IFetchBlogService
{
    Task<Blog> FetchBlog(int publisherId, int tenantId, HttpContext context, int startIndex, int length);
}

