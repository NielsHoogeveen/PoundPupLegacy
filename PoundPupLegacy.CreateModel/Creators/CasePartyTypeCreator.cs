namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CasePartyTypeCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<NewCasePartyType> casePartyTypeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewCasePartyType>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewCasePartyType> casePartyTypes, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var casePartyTypeWriter = await casePartyTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var casePartyType in casePartyTypes) {
            await nodeWriter.InsertAsync(casePartyType);
            await searchableWriter.InsertAsync(casePartyType);
            await nameableWriter.InsertAsync(casePartyType);
            await casePartyTypeWriter.InsertAsync(casePartyType);
            await WriteTerms(casePartyType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in casePartyType.TenantNodes) {
                tenantNode.NodeId = casePartyType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
