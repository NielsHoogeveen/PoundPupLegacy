using SearchOption = PoundPupLegacy.ViewModel.Models.SearchOption;

namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchTopicsService
{
    [RequireNamedArgs]
    Task<Topics> FetchTopics(int userId, int tenantId, int pageSize, int pageNumber, string searchTerm, SearchOption searchOption);
}
