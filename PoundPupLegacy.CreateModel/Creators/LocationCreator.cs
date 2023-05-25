namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class LocationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableLocation> locationInserterFactory
) : IEntityCreatorFactory<EventuallyIdentifiableLocation>
{
    public async Task<IEntityCreator<EventuallyIdentifiableLocation>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<EventuallyIdentifiableLocation>(
            new() 
            {
                await locationInserterFactory.CreateAsync(connection)
            }
        );
}
