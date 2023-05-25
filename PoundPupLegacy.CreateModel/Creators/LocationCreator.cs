namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class LocationCreatorFactory(
    IDatabaseInserterFactory<Location> locationInserterFactory,
    IDatabaseInserterFactory<LocationLocatable> locationLocatableInserterFactory
) : IEntityCreatorFactory<Location>
{
    public async Task<IEntityCreator<Location>> CreateAsync(IDbConnection connection) =>
        new LocationCreator(
            new() 
            {
                await locationInserterFactory.CreateAsync(connection)
            },
            await locationLocatableInserterFactory.CreateAsync(connection)
        );
}

public class LocationCreator(
    List<IDatabaseInserter<Location>> inserters,
    IDatabaseInserter<LocationLocatable> locationLocatableInserter
) : InsertingEntityCreator<Location>(inserters)
{
    public override async Task ProcessAsync(Location element)
    {
        await base.ProcessAsync(element);
        foreach (var locationLocatable in element.Locatables) {
            locationLocatable.LocationId = element.Id;
            await locationLocatableInserter.InsertAsync(locationLocatable);
        }
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await locationLocatableInserter.DisposeAsync();
    }
}
