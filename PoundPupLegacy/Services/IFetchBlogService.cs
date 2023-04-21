using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface IFetchBlogService
{
    [RequireNamedArgs]
    Task<Blog?> FetchBlog(int publisherId, int tenantId, int pageNumber, int pageSize);
}

