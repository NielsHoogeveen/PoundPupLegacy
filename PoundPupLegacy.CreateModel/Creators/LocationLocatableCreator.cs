using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class LocationLocatableCreatorFactory(
    IDatabaseInserterFactory<LocationLocatable> locationLocatableInserterFactory
    ) : IEntityCreatorFactory<LocationLocatable>
{
    public async Task<IEntityCreator<LocationLocatable>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<LocationLocatable>(new()
        {
            await locationLocatableInserterFactory.CreateAsync(connection)
        });
}
