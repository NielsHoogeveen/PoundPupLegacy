namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchBlogService
{
    [RequireNamedArgs]
    Task<Blog?> FetchBlog(int publisherId, int tenantId, int pageNumber, int pageSize);
}

