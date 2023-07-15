namespace PoundPupLegacy.EditModel.Mappers;

internal class LocatableDetailsForCreateMapper(
    IEnumerableMapper<Location.ToCreate, DomainModel.Location.ToCreate> locationToAddMapper
) : IMapper<LocatableDetails.ForCreate, DomainModel.LocatableDetails.ForCreate>
{
    public DomainModel.LocatableDetails.ForCreate Map(LocatableDetails.ForCreate source)
    {
        return new DomainModel.LocatableDetails.ForCreate {
            Locations = locationToAddMapper.Map(source.LocationsToAdd).ToList(),
        };
    }
}
