namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchBlogsService
{
    Task<List<BlogListEntry>> FetchBlogs(int tenantId);
}

