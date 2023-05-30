using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class LocationsToUpdateMapper : IEnumerableMapper<Location.ExistingLocation, ImmediatelyIdentifiableLocation>
{
    public IEnumerable<ImmediatelyIdentifiableLocation> Map(IEnumerable<Location.ExistingLocation> source)
    {
        foreach(var location in source) {
            if(location.HasBeenDeleted)
                continue;
            yield return new CreateModel.LocationToUpdate {
                IdentificationCertain = new Identification.Certain { 
                    Id = location.Id,
                },
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
