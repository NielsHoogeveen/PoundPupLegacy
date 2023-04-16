using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface IFetchBlogsService
{
    Task<List<BlogListEntry>> FetchBlogs(int tenantId);
}

