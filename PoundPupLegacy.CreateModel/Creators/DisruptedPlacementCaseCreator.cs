namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DisruptedPlacementCaseCreator(
    IDatabaseInserterFactory<DisruptedPlacementCase> disruptedPlacementCaseInserterFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Locatable> locatableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Case> caseInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<DisruptedPlacementCase>
{

    public override async Task CreateAsync(IAsyncEnumerable<DisruptedPlacementCase> disruptedPlacementCases, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var caseWriter = await caseInserterFactory.CreateAsync(connection);
        await using var disruptedPlacementCaseWriter = await disruptedPlacementCaseInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var disruptedPlacementCase in disruptedPlacementCases) {
            await nodeWriter.InsertAsync(disruptedPlacementCase);
            await searchableWriter.InsertAsync(disruptedPlacementCase);
            await documentableWriter.InsertAsync(disruptedPlacementCase);
            await locatableWriter.InsertAsync(disruptedPlacementCase);
            await nameableWriter.InsertAsync(disruptedPlacementCase);
            await caseWriter.InsertAsync(disruptedPlacementCase);
            await disruptedPlacementCaseWriter.InsertAsync(disruptedPlacementCase);
            await WriteTerms(disruptedPlacementCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in disruptedPlacementCase.TenantNodes) {
                tenantNode.NodeId = disruptedPlacementCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
