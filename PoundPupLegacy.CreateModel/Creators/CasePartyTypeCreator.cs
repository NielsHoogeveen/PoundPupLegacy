namespace PoundPupLegacy.CreateModel.Creators;

public class CasePartyTypeCreator : IEntityCreator<CasePartyType>
{
    public static async Task CreateAsync(IAsyncEnumerable<CasePartyType> casePartyTypes, NpgsqlConnection connection)
    {
        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var casePartyTypeWriter = await CasePartyTypeInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

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
