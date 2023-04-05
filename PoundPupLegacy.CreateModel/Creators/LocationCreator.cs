namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class LocationCreator : EntityCreator<Location>
{
    private readonly IDatabaseInserterFactory<Location> _locationInserterFactory;
    private readonly IDatabaseInserterFactory<LocationLocatable> _locationLocatableInserterFactory;
    public LocationCreator(
               IDatabaseInserterFactory<Location> locationInserterFactory,
                      IDatabaseInserterFactory<LocationLocatable> locationLocatableInserterFactory
           )
    {
        _locationInserterFactory = locationInserterFactory;
        _locationLocatableInserterFactory = locationLocatableInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<Location> locations, IDbConnection connection)
    {

        await using var locationWriter = await _locationInserterFactory.CreateAsync(connection);
        await using var locationLocatableWriter = await _locationLocatableInserterFactory.CreateAsync(connection);

        await foreach (var location in locations) {
            await locationWriter.InsertAsync(location);
            foreach (var locationLocatable in location.Locatables) {
                await locationLocatableWriter.InsertAsync(locationLocatable);
            }
        }
    }
}
