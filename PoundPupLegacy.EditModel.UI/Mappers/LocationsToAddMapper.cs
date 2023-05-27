using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class LocationsToAddMapper : IEnumerableMapper<Location.ExistingLocation, EventuallyIdentifiableLocation>
{
    public IEnumerable<EventuallyIdentifiableLocation> Map(IEnumerable<Location.ExistingLocation> source)
    {
        foreach (var location in source) {
            yield return new CreateModel.NewLocation {
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
