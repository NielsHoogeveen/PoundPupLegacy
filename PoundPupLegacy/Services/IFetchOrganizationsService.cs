using PoundPupLegacy.ViewModel;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Services;

public interface IFetchOrganizationsService
{
    public Task<OrganizationSearch> FetchOrganizations(int limit, int offset, string searchTerm, SearchOption searchOption, int tenantId, int userId, int? organizationTypeId, int? countryId);
}
