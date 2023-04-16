using PoundPupLegacy.ViewModel.Models;
using SearchOption = PoundPupLegacy.ViewModel.Models.SearchOption;

namespace PoundPupLegacy.Services;

public interface IFetchOrganizationsService
{
    public Task<OrganizationSearch> FetchOrganizations(int userId, int tenantId, int limit, int offset, string searchTerm, SearchOption searchOption, int? organizationTypeId, int? countryId);
}
