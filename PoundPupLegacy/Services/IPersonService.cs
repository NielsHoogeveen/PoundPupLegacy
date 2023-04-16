using PoundPupLegacy.ViewModel.Models;
using SearchOption = PoundPupLegacy.ViewModel.Models.SearchOption;

namespace PoundPupLegacy.Services;

public interface IPersonService
{
    Task<Persons> FetchPersons(int userId, int tenantId, int limit, int offset, string searchTerm, SearchOption searchOption);
}
