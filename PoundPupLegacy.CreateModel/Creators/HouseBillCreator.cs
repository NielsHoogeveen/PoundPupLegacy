namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HouseBillCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<BillToCreate> billInserterFactory,
    IDatabaseInserterFactory<HouseBill.ToCreate> houseBillInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<HouseBill.ToCreate>
{
    public async Task<IEntityCreator<HouseBill.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<HouseBill.ToCreate>(
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
