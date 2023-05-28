namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BillActionTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableBillActionType> billActionTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory

) : IEntityCreatorFactory<EventuallyIdentifiableBillActionType>
{
    public async Task<IEntityCreator<EventuallyIdentifiableBillActionType>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<EventuallyIdentifiableBillActionType>(
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
