namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class LocationCreator(
    IDatabaseInserterFactory<Location> locationInserterFactory,
    IDatabaseInserterFactory<LocationLocatable> locationLocatableInserterFactory
) : EntityCreator<Location>
{
    public override async Task CreateAsync(IAsyncEnumerable<Location> locations, IDbConnection connection)
    {
        await using var locationWriter = await locationInserterFactory.CreateAsync(connection);
        await using var locationLocatableWriter = await locationLocatableInserterFactory.CreateAsync(connection);

        await foreach (var location in locations) {
            await locationWriter.InsertAsync(location);
            foreach (var locationLocatable in location.Locatables) {
                locationLocatable.LocationId = location.Id;
                await locationLocatableWriter.InsertAsync(locationLocatable);
            }
        }
    }
}
