namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class LocatableDetailsForCreateMapper(
    IEnumerableMapper<EditModel.Location.ToCreate, CreateModel.Location.ToCreate> locationToAddMapper
) : IMapper<EditModel.LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate>
{
    public CreateModel.LocatableDetails.ForCreate Map(LocatableDetails.ForCreate source)
    {
        return new CreateModel.LocatableDetails.ForCreate {
            Locations = locationToAddMapper.Map(source.LocationsToAdd).ToList(),
        };
    }
}
