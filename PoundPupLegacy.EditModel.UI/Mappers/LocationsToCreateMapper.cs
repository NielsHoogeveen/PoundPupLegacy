namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class LocationsToCreateMapper : IEnumerableMapper<EditModel.Location.ToCreate, CreateModel.Location.ToCreate>
{
    public IEnumerable<CreateModel.Location.ToCreate> Map(IEnumerable<Location.ToCreate> source)
    {
        foreach (var location in source) {
            yield return new CreateModel.Location.ToCreate {
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
