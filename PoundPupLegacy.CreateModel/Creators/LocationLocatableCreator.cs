namespace PoundPupLegacy.CreateModel.Creators;

public class LocationLocatableCreator : IEntityCreator<LocationLocatable>
{
    public async Task CreateAsync(IAsyncEnumerable<LocationLocatable> locationLocatables, IDbConnection connection)
    {

        await using var locationLocatableWriter = await LocationLocatableInserter.CreateAsync(connection);

        await foreach (var locationLocatable in locationLocatables) {
            await locationLocatableWriter.InsertAsync(locationLocatable);
        }
    }
}
