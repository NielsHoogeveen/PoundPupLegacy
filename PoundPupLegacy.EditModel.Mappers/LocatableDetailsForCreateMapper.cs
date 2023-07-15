namespace PoundPupLegacy.EditModel.Mappers;

internal class LocatableDetailsForCreateMapper(
    IEnumerableMapper<Location.ToCreate, CreateModel.Location.ToCreate> locationToAddMapper
) : IMapper<LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate>
{
    public CreateModel.LocatableDetails.ForCreate Map(LocatableDetails.ForCreate source)
    {
        return new CreateModel.LocatableDetails.ForCreate {
            Locations = locationToAddMapper.Map(source.LocationsToAdd).ToList(),
        };
    }
}
