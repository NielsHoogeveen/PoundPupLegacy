using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Services;

public interface ILocationService
{
    Task<Location> ValidateLocationAsync(Location location);
    IAsyncEnumerable<SubdivisionListItem> SubdivisionsOfCountry(int countryId);
    IAsyncEnumerable<CountryListItem> Countries();
}
