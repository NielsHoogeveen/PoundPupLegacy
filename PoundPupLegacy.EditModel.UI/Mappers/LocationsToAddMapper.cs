using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class LocationsToAddMapper : IEnumerableMapper<Location, EventuallyIdentifiableLocation>
{
    public IEnumerable<EventuallyIdentifiableLocation> Map(IEnumerable<Location> source)
    {
        foreach (var location in source) {
            if (location.LocationId.HasValue)
                continue;
            yield return new NewLocation {
                Id = null,
                Additional = location.Addition,
                City = location.City,
                CountryId = location.CountryId,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                PostalCode = location.PostalCode,
                Street = location.Street,
                SubdivisionId = location.SubdivisionId,
            };
        }
    }
}
