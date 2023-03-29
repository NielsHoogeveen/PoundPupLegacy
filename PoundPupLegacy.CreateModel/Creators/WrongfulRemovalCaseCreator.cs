namespace PoundPupLegacy.CreateModel.Creators;

public class WrongfulRemovalCaseCreator : IEntityCreator<WrongfulRemovalCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<WrongfulRemovalCase> wrongfulRemovalCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var locatableWriter = await LocatableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var caseWriter = await CaseInserter.CreateAsync(connection);
        await using var wrongfulRemovalCaseWriter = await WrongfulRemovalCaseInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var wrongfulRemovalCase in wrongfulRemovalCases) {
            await nodeWriter.WriteAsync(wrongfulRemovalCase);
            await searchableWriter.WriteAsync(wrongfulRemovalCase);
            await documentableWriter.WriteAsync(wrongfulRemovalCase);
            await locatableWriter.WriteAsync(wrongfulRemovalCase);
            await nameableWriter.WriteAsync(wrongfulRemovalCase);
            await caseWriter.WriteAsync(wrongfulRemovalCase);
            await wrongfulRemovalCaseWriter.WriteAsync(wrongfulRemovalCase);
            await EntityCreator.WriteTerms(wrongfulRemovalCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in wrongfulRemovalCase.TenantNodes) {
                tenantNode.NodeId = wrongfulRemovalCase.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
