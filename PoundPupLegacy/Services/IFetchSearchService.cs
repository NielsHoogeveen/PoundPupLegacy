using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface IFetchSearchService
{
    Task<SearchResult> FetchSearch(int userId, int tenantId, int pageSize, int pageNumber, string searchString);
}
