namespace PoundPupLegacy.EditModel.Mappers;

internal class LocatableDetailsForUpdateMapper(
    IEnumerableMapper<Location.ToCreate, DomainModel.Location.ToCreate> locationToAddMapper,
    IEnumerableMapper<Location.ToUpdate, int> locationsToDeleteMapper,
    IEnumerableMapper<Location.ToUpdate, DomainModel.Location.ToUpdate> locationsToUpdateMapper
) : IMapper<LocatableDetails.ForUpdate, DomainModel.LocatableDetails.ForUpdate>
{
    public DomainModel.LocatableDetails.ForUpdate Map(LocatableDetails.ForUpdate source)
    {
        return new DomainModel.LocatableDetails.ForUpdate {
            LocationsToAdd = locationToAddMapper.Map(source.LocationsToAdd).ToList(),
            LocationsToDelete = locationsToDeleteMapper.Map(source.LocationsToUpdate).ToList(),
            LocationsToUpdate = locationsToUpdateMapper.Map(source.LocationsToUpdate).ToList(),
        };
    }
}
