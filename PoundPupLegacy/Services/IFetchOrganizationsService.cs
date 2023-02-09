using PoundPupLegacy.ViewModel;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Services;

public interface IFetchOrganizationsService
{
    public Task<Organizations> FetchOrganizations(int limit, int offset, string searchTerm, SearchOption searchOption, int tenantId, int userId);
}
