namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CoercedAdoptionCaseCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Locatable> locatableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Case> caseInserterFactory,
    IDatabaseInserterFactory<CoercedAdoptionCase> coercedAdoptionCaseInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<CoercedAdoptionCase>
{
    public override async Task CreateAsync(IAsyncEnumerable<CoercedAdoptionCase> coercedAdoptionCases, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var caseWriter = await caseInserterFactory.CreateAsync(connection);
        await using var coercedAdoptionCaseWriter = await coercedAdoptionCaseInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var coercedAdoptionCase in coercedAdoptionCases) {
            await nodeWriter.InsertAsync(coercedAdoptionCase);
            await searchableWriter.InsertAsync(coercedAdoptionCase);
            await documentableWriter.InsertAsync(coercedAdoptionCase);
            await locatableWriter.InsertAsync(coercedAdoptionCase);
            await nameableWriter.InsertAsync(coercedAdoptionCase);
            await caseWriter.InsertAsync(coercedAdoptionCase);
            await coercedAdoptionCaseWriter.InsertAsync(coercedAdoptionCase);
            await WriteTerms(coercedAdoptionCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in coercedAdoptionCase.TenantNodes) {
                tenantNode.NodeId = coercedAdoptionCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
