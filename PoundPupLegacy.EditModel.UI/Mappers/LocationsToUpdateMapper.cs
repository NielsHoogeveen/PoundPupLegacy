using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class LocationsToUpdateMapper : IEnumerableMapper<Location, ImmediatelyIdentifiableLocation>
{
    public IEnumerable<ImmediatelyIdentifiableLocation> Map(IEnumerable<Location> source)
    {
        foreach(var location in source) {
            if (!location.LocationId.HasValue)
                continue;
            yield return new ExistingLocation { 
                Id = location.LocationId.Value,
                Additional = location.Addition,
                City = location.City,
                CountryId = location.CountryId,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                PostalCode  = location.PostalCode,
                Street = location.Street,
                SubdivisionId  = location.SubdivisionId,
            };
        }
    }
}
