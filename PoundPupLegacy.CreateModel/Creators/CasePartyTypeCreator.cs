namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CasePartyTypeCreator : IEntityCreator<CasePartyType>
{
    public async Task CreateAsync(IAsyncEnumerable<CasePartyType> casePartyTypes, IDbConnection connection)
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
            await nodeWriter.InsertAsync(casePartyType);
            await searchableWriter.InsertAsync(casePartyType);
            await nameableWriter.InsertAsync(casePartyType);
            await casePartyTypeWriter.InsertAsync(casePartyType);
            await EntityCreator.WriteTerms(casePartyType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in casePartyType.TenantNodes) {
                tenantNode.NodeId = casePartyType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
