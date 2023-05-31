namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class LocatableDetailsForUpdateMapper(
    IEnumerableMapper<EditModel.Location.ToCreate, CreateModel.Location.ToCreate> locationToAddMapper,
    IEnumerableMapper<EditModel.Location.ToUpdate, int> locationsToDeleteMapper,
    IEnumerableMapper<Location.ToUpdate, CreateModel.Location.ToUpdate> locationsToUpdateMapper
) : IMapper<EditModel.LocatableDetails.ForUpdate, CreateModel.LocatableDetails.ForUpdate>
{
    public CreateModel.LocatableDetails.ForUpdate Map(LocatableDetails.ForUpdate source)
    {
        return new CreateModel.LocatableDetails.ForUpdate {
            LocationsToAdd = locationToAddMapper.Map(source.LocationsToAdd).ToList(),
            LocationsToDelete = locationsToDeleteMapper.Map(source.LocationsToUpdate).ToList(),
            LocationsToUpdate = locationsToUpdateMapper.Map(source.LocationsToUpdate).ToList(),
        };
    }
}
