namespace PoundPupLegacy.CreateModel.Creators;

public class ChildTraffickingCaseCreator : IEntityCreator<ChildTraffickingCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<ChildTraffickingCase> childTraffickingCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var locatableWriter = await LocatableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var caseWriter = await CaseInserter.CreateAsync(connection);
        await using var childTraffickingCaseWriter = await ChildTraffickingCaseInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var childTraffickingCase in childTraffickingCases) {
            await nodeWriter.InsertAsync(childTraffickingCase);
            await searchableWriter.InsertAsync(childTraffickingCase);
            await documentableWriter.InsertAsync(childTraffickingCase);
            await locatableWriter.InsertAsync(childTraffickingCase);
            await nameableWriter.InsertAsync(childTraffickingCase);
            await caseWriter.InsertAsync(childTraffickingCase);
            await childTraffickingCaseWriter.InsertAsync(childTraffickingCase);
            await EntityCreator.WriteTerms(childTraffickingCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in childTraffickingCase.TenantNodes) {
                tenantNode.NodeId = childTraffickingCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
