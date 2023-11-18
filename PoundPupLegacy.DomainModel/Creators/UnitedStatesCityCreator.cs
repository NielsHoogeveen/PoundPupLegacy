using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class UnitedStatesCityCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<UnitedStatesCity.ToCreate> cityInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<UnitedStatesCity.ToCreate>
{
    public async Task<IEntityCreator<UnitedStatesCity.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<UnitedStatesCity.ToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await cityInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
