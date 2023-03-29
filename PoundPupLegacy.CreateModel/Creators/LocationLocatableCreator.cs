namespace PoundPupLegacy.CreateModel.Creators;

public class LocationLocatableCreator : IEntityCreator<LocationLocatable>
{
    public static async Task CreateAsync(IAsyncEnumerable<LocationLocatable> locationLocatables, NpgsqlConnection connection)
    {

        await using var locationLocatableWriter = await LocationLocatableInserter.CreateAsync(connection);

        await foreach (var locationLocatable in locationLocatables) {
            await locationLocatableWriter.WriteAsync(locationLocatable);
        }
    }
}
