using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class DisruptedPlacementCaseCreator : IEntityCreator<DisruptedPlacementCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<DisruptedPlacementCase> disruptedPlacementCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var caseWriter = await CaseWriter.CreateAsync(connection);
        await using var disruptedPlacementCaseWriter = await DisruptedPlacementCaseWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

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
