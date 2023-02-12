using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface IFetchBlogService
{
    Task<Blog> FetchBlog(int publisherId, int startIndex, int length);
}

