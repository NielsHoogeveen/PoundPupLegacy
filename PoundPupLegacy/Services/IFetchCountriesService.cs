using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface IFetchCountriesService
{
    Task<FirstLevelRegionListEntry[]> FetchCountries();
}
