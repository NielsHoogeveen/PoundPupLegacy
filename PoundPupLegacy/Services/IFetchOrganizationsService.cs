using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using SearchOption = PoundPupLegacy.ViewModel.Models.SearchOption;

namespace PoundPupLegacy.Services;

public interface IFetchOrganizationsService
{
    [RequireNamedArgs]
    public Task<OrganizationSearch> FetchOrganizations(int userId, int tenantId, int pageSize, int pageNumber, string searchTerm, SearchOption searchOption, int? organizationTypeId, int? countryId);
}
