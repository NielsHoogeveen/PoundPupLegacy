using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface IFetchBlogService
{
    Task<Blog> FetchBlog(int publisherId, int tenantId, int startIndex, int length);
}

