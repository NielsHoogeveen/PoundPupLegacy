using PoundPupLegacy.ViewModel;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Services;

public interface IPersonService
{
    Task<Persons> FetchPersons(int limit, int offset, string searchTerm, SearchOption searchOption);
}
