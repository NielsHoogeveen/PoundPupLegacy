using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class LocationsToUpdateMapper : IEnumerableMapper<Location.ToUpdate, CreateModel.Location.ToUpdate>
{
    public IEnumerable<CreateModel.Location.ToUpdate> Map(IEnumerable<Location.ToUpdate> source)
    {
        foreach (var location in source) {
            if (location.HasBeenDeleted)
                continue;
            yield return new CreateModel.Location.ToUpdate {
                Identification = new Identification.Certain {
                    Id = location.Id,
                },
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
