namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class LocationLocatableCreator : EntityCreator<LocationLocatable>
{
    private readonly IDatabaseInserterFactory<LocationLocatable> _locationLocatableInserterFactory;
    public LocationLocatableCreator(IDatabaseInserterFactory<LocationLocatable> locationLocatableInserterFactory)
    {
        _locationLocatableInserterFactory = locationLocatableInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<LocationLocatable> locationLocatables, IDbConnection connection)
    {

        await using var locationLocatableWriter = await _locationLocatableInserterFactory.CreateAsync(connection);

        await foreach (var locationLocatable in locationLocatables) {
            await locationLocatableWriter.InsertAsync(locationLocatable);
        }
    }
}
