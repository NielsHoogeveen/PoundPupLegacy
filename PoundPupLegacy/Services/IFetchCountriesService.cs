using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface IFetchCountriesService
{
    Task<FirstLevelRegionListEntry[]> FetchCountries(int tenantId);
}
