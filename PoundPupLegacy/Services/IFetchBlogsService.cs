using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface IFetchBlogsService
{
    Task<List<BlogListEntry>> FetchBlogs();
}

