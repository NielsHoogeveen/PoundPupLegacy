namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchCountriesService
{
    Task<FirstLevelRegionListEntry[]> FetchCountries(int tenantId);
}
