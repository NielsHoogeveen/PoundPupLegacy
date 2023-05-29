namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HagueStatusCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<HagueStatus.HagueStatusToCreate> hagueStatusInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<HagueStatus.HagueStatusToCreate>
{
    public async Task<IEntityCreator<HagueStatus.HagueStatusToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<HagueStatus.HagueStatusToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await hagueStatusInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
