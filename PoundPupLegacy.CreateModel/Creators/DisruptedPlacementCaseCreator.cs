namespace PoundPupLegacy.CreateModel.Creators;

public class DisruptedPlacementCaseCreator : IEntityCreator<DisruptedPlacementCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<DisruptedPlacementCase> disruptedPlacementCases, NpgsqlConnection connection)
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
            await nodeWriter.WriteAsync(disruptedPlacementCase);
            await searchableWriter.WriteAsync(disruptedPlacementCase);
            await documentableWriter.WriteAsync(disruptedPlacementCase);
            await locatableWriter.WriteAsync(disruptedPlacementCase);
            await nameableWriter.WriteAsync(disruptedPlacementCase);
            await caseWriter.WriteAsync(disruptedPlacementCase);
            await disruptedPlacementCaseWriter.WriteAsync(disruptedPlacementCase);
            await EntityCreator.WriteTerms(disruptedPlacementCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in disruptedPlacementCase.TenantNodes) {
                tenantNode.NodeId = disruptedPlacementCase.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
