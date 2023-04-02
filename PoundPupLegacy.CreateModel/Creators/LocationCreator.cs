namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class LocationCreator : IEntityCreator<Location>
{
    public async Task CreateAsync(IAsyncEnumerable<Location> locations, IDbConnection connection)
    {

        await using var locationWriter = await LocationInserter.CreateAsync(connection);
        await using var locationLocatableWriter = await LocationLocatableInserter.CreateAsync(connection);

        await foreach (var location in locations) {
            await locationWriter.InsertAsync(location);
            foreach (var locationLocatable in location.Locatables) {
                await locationLocatableWriter.InsertAsync(locationLocatable);
            }
        }
    }
}
