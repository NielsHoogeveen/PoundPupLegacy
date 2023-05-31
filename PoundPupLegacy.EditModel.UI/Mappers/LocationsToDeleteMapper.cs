namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class LocationsToDeleteMapper : IEnumerableMapper<EditModel.Location.ToUpdate, int>
{
    public IEnumerable<int> Map(IEnumerable<EditModel.Location.ToUpdate> source)
    {
        foreach (var location in source) {
            if (!location.HasBeenDeleted)
                continue;
            yield return location.Id;
        }
    }
}
