namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BillActionTypeCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<NewBillActionType> billActionTypeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewBillActionType>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewBillActionType> billActionTypes, IDbConnection connection)
    {

        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var billActionTypeWriter = await billActionTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var billActionType in billActionTypes) {
            await nodeWriter.InsertAsync(billActionType);
            await searchableWriter.InsertAsync(billActionType);
            await nameableWriter.InsertAsync(billActionType);
            await billActionTypeWriter.InsertAsync(billActionType);
            await WriteTerms(billActionType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in billActionType.TenantNodes) {
                tenantNode.NodeId = billActionType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
