using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface IFetchSearchService
{
    [RequireNamedArgs]
    Task<SearchResult> FetchSearch(int userId, int tenantId, int pageSize, int pageNumber, string searchString);
}
