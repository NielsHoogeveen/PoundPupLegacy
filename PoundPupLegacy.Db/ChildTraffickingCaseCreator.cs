using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class ChildTraffickingCaseCreator : IEntityCreator<ChildTraffickingCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<ChildTraffickingCase> childTraffickingCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var caseWriter = await CaseWriter.CreateAsync(connection);
        await using var childTraffickingCaseWriter = await ChildTraffickingCaseWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var childTraffickingCase in childTraffickingCases)
        {
            await nodeWriter.WriteAsync(childTraffickingCase);
            await searchableWriter.WriteAsync(childTraffickingCase);
            await documentableWriter.WriteAsync(childTraffickingCase);
            await locatableWriter.WriteAsync(childTraffickingCase);
            await nameableWriter.WriteAsync(childTraffickingCase);
            await caseWriter.WriteAsync(childTraffickingCase);
            await childTraffickingCaseWriter.WriteAsync(childTraffickingCase);
            await EntityCreator.WriteTerms(childTraffickingCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in childTraffickingCase.TenantNodes)
            {
                tenantNode.NodeId = childTraffickingCase.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
