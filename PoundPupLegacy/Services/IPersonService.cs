using PoundPupLegacy.ViewModel;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Services;

public interface IPersonService
{
    Task<Persons> FetchPersons(int userId, int tenantId, int limit, int offset, string searchTerm, SearchOption searchOption);
}
