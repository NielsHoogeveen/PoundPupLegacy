namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class LocationsToRemoveMapper : IEnumerableMapper<ExistingLocation, int>
{
    public IEnumerable<int> Map(IEnumerable<ExistingLocation> source)
    {
        foreach (var location in source) {
            if (!location.HasBeenDeleted)
                continue;
            yield return location.Id;
        }
    }
}
