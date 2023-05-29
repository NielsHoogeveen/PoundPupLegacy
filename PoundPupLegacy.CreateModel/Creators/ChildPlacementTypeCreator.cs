namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ChildPlacementTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<ChildPlacementType.ChildPlacementTypeToCreate> childPlacementTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<ChildPlacementType.ChildPlacementTypeToCreate>
{
    public async Task<IEntityCreator<ChildPlacementType.ChildPlacementTypeToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<ChildPlacementType.ChildPlacementTypeToCreate>(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await childPlacementTypeInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
