namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchBlogService
{
    [RequireNamedArgs]
    Task<Blog> FetchBlog(int publisherId, int tenantId, int userId, int pageNumber, int pageSize);
}

