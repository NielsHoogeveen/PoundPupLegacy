namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BillActionTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<BillActionType.BillActionTypeToCreate> billActionTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory

) : IEntityCreatorFactory<BillActionType.BillActionTypeToCreate>
{
    public async Task<IEntityCreator<BillActionType.BillActionTypeToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<BillActionType.BillActionTypeToCreate>(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await billActionTypeInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
