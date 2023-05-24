namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class LocationLocatableCreatorFactory(
    IDatabaseInserterFactory<LocationLocatable> locationLocatableInserterFactory
    ) : IInsertingEntityCreatorFactory<LocationLocatable>
{
    public async Task<InsertingEntityCreator<LocationLocatable>> CreateAsync(IDbConnection connection) => 
        new( new() 
        { 
            await locationLocatableInserterFactory.CreateAsync(connection) 
        });
}
