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
            await nodeWriter.InsertAsync(wrongfulRemovalCase);
            await searchableWriter.InsertAsync(wrongfulRemovalCase);
            await documentableWriter.InsertAsync(wrongfulRemovalCase);
            await locatableWriter.InsertAsync(wrongfulRemovalCase);
            await nameableWriter.InsertAsync(wrongfulRemovalCase);
            await caseWriter.InsertAsync(wrongfulRemovalCase);
            await wrongfulRemovalCaseWriter.InsertAsync(wrongfulRemovalCase);
            await EntityCreator.WriteTerms(wrongfulRemovalCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in wrongfulRemovalCase.TenantNodes) {
                tenantNode.NodeId = wrongfulRemovalCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
