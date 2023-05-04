using SearchOption = PoundPupLegacy.Common.SearchOption;

namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchPersonService
{
    [RequireNamedArgs]
    Task<Persons> FetchPersons(int userId, int tenantId, int pageSize, int pageNumber, string searchTerm, SearchOption searchOption);
}
