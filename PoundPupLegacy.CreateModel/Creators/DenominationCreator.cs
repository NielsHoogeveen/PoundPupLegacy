namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DenominationCreator(
    IDatabaseInserterFactory<NewDenomination> denominationInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewDenomination>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewDenomination> denominations, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var denominationWriter = await denominationInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var denomination in denominations) {
            await nodeWriter.InsertAsync(denomination);
            await searchableWriter.InsertAsync(denomination);
            await nameableWriter.InsertAsync(denomination);
            await denominationWriter.InsertAsync(denomination);
            await WriteTerms(denomination, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in denomination.TenantNodes) {
                tenantNode.NodeId = denomination.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
