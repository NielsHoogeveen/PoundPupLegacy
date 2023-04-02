namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DenominationCreator : IEntityCreator<Denomination>
{
    public async Task CreateAsync(IAsyncEnumerable<Denomination> denominations, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var denominationWriter = await DenominationInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var denomination in denominations) {
            await nodeWriter.InsertAsync(denomination);
            await searchableWriter.InsertAsync(denomination);
            await nameableWriter.InsertAsync(denomination);
            await denominationWriter.InsertAsync(denomination);
            await EntityCreator.WriteTerms(denomination, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in denomination.TenantNodes) {
                tenantNode.NodeId = denomination.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
