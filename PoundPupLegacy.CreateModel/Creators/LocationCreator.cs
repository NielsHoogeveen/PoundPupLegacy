namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class LocationCreatorFactory(
    IDatabaseInserterFactory<LocationToCreate> locationInserterFactory
) : IEntityCreatorFactory<LocationToCreate>
{
    public async Task<IEntityCreator<LocationToCreate>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<LocationToCreate>(
            new() 
            {
                await locationInserterFactory.CreateAsync(connection)
            }
        );
}
