namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchSearchService
{
    [RequireNamedArgs]
    Task<SearchResult> FetchSearch(int userId, int tenantId, int pageSize, int pageNumber, string searchString);
}
