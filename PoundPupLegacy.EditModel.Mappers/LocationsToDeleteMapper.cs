namespace PoundPupLegacy.EditModel.Mappers;

internal class LocationsToDeleteMapper : IEnumerableMapper<Location.ToUpdate, int>
{
    public IEnumerable<int> Map(IEnumerable<Location.ToUpdate> source)
    {
        foreach (var location in source) {
            if (!location.HasBeenDeleted)
                continue;
            yield return location.Id;
        }
    }
}
