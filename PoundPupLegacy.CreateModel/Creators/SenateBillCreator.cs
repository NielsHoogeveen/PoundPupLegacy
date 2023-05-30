namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenateBillCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<BillToCreate> billInserterFactory,
    IDatabaseInserterFactory<SenateBill.ToCreate> senateBillInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<SenateBill.ToCreate>
{
    public async Task<IEntityCreator<SenateBill.ToCreate>> CreateAsync(IDbConnection connection) => 
        new NameableCreator<SenateBill.ToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await billInserterFactory.CreateAsync(connection),
                await senateBillInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
