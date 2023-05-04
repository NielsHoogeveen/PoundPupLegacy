using SearchOption = PoundPupLegacy.Common.SearchOption;

namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchOrganizationsService
{
    [RequireNamedArgs]
    public Task<OrganizationSearch> FetchOrganizations(int userId, int tenantId, int pageSize, int pageNumber, string searchTerm, SearchOption searchOption, int? organizationTypeId, int? countryId);
}
