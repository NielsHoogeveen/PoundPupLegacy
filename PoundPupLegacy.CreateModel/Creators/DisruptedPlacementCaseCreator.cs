namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DisruptedPlacementCaseCreator : IEntityCreator<DisruptedPlacementCase>
{
    public async Task CreateAsync(IAsyncEnumerable<DisruptedPlacementCase> disruptedPlacementCases, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var locatableWriter = await LocatableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var caseWriter = await CaseInserter.CreateAsync(connection);
        await using var disruptedPlacementCaseWriter = await DisruptedPlacementCaseInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var disruptedPlacementCase in disruptedPlacementCases) {
            await nodeWriter.InsertAsync(disruptedPlacementCase);
            await searchableWriter.InsertAsync(disruptedPlacementCase);
            await documentableWriter.InsertAsync(disruptedPlacementCase);
            await locatableWriter.InsertAsync(disruptedPlacementCase);
            await nameableWriter.InsertAsync(disruptedPlacementCase);
            await caseWriter.InsertAsync(disruptedPlacementCase);
            await disruptedPlacementCaseWriter.InsertAsync(disruptedPlacementCase);
            await EntityCreator.WriteTerms(disruptedPlacementCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in disruptedPlacementCase.TenantNodes) {
                tenantNode.NodeId = disruptedPlacementCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
