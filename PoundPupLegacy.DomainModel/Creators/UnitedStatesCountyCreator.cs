using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class UnitedStatesCountyCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<UnitedStatesCounty.ToCreate> countyInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<UnitedStatesCounty.ToCreate>
{
    public async Task<IEntityCreator<UnitedStatesCounty.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<UnitedStatesCounty.ToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await countyInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
