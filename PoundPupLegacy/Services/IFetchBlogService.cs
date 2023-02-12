using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface IFetchBlogService
{
    Task<Blog> FetchBlog(int accessRoleId, int startIndex, int length);
}

