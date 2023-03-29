namespace PoundPupLegacy.CreateModel.Creators;

public class CasePartyTypeCreator : IEntityCreator<CasePartyType>
{
    public static async Task CreateAsync(IAsyncEnumerable<CasePartyType> casePartyTypes, NpgsqlConnection connection)
    {
        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var casePartyTypeWriter = await CasePartyTypeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await (new TermReaderByNameFactory()).CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await (new VocabularyIdReaderByOwnerAndNameFactory()).CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var casePartyType in casePartyTypes) {
            await nodeWriter.WriteAsync(casePartyType);
            await searchableWriter.WriteAsync(casePartyType);
            await nameableWriter.WriteAsync(casePartyType);
            await casePartyTypeWriter.WriteAsync(casePartyType);
            await EntityCreator.WriteTerms(casePartyType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in casePartyType.TenantNodes) {
                tenantNode.NodeId = casePartyType.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
