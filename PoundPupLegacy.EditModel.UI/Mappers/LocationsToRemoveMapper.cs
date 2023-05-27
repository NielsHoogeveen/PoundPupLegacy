namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class LocationsToRemoveMapper : IEnumerableMapper<Location, int>
{
    public IEnumerable<int> Map(IEnumerable<Location> source)
    {
        foreach (var location in source) {
            if (!location.LocationId.HasValue)
                continue;
            if (!location.HasBeenDeleted)
                continue;
            yield return location.LocationId.Value;
        }
    }
}
