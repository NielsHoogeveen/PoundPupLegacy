namespace PoundPupLegacy.EditModel.Mappers;

internal class LocationsToCreateMapper : IEnumerableMapper<Location.ToCreate, DomainModel.Location.ToCreate>
{
    public IEnumerable<DomainModel.Location.ToCreate> Map(IEnumerable<Location.ToCreate> source)
    {
        foreach (var location in source) {
            yield return new DomainModel.Location.ToCreate {
                Identification = new Identification.Possible {
                    Id = null,
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
