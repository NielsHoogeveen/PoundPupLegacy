namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HouseBillCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<BillToCreate> billInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableHouseBill> houseBillInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableHouseBill>
{
    public async Task<IEntityCreator<EventuallyIdentifiableHouseBill>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<EventuallyIdentifiableHouseBill>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await billInserterFactory.CreateAsync(connection),
                await houseBillInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
