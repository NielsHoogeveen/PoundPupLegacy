namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class LocationCreatorFactory(
    IDatabaseInserterFactory<Location.ToCreate> locationInserterFactory
) : IEntityCreatorFactory<Location.ToCreate>
{
    public async Task<IEntityCreator<Location.ToCreate>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<Location.ToCreate>(
            new()
            {
                await locationInserterFactory.CreateAsync(connection)
            }
        );
}
