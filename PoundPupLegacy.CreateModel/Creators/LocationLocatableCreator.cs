namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class LocationLocatableCreator(IDatabaseInserterFactory<LocationLocatable> locationLocatableInserterFactory) : EntityCreator<LocationLocatable>
{
    public override async Task CreateAsync(IAsyncEnumerable<LocationLocatable> locationLocatables, IDbConnection connection)
    {
        await using var locationLocatableWriter = await locationLocatableInserterFactory.CreateAsync(connection);

        await foreach (var locationLocatable in locationLocatables) {
            await locationLocatableWriter.InsertAsync(locationLocatable);
        }
    }
}
