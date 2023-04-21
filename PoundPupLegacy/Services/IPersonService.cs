using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using SearchOption = PoundPupLegacy.ViewModel.Models.SearchOption;

namespace PoundPupLegacy.Services;

public interface IPersonService
{
    [RequireNamedArgs]
    Task<Persons> FetchPersons(int userId, int tenantId, int pageSize, int pageNumber, string searchTerm, SearchOption searchOption);
}
