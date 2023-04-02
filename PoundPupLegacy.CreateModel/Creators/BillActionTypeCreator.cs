namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BillActionTypeCreator : IEntityCreator<BillActionType>
{
    public async Task CreateAsync(IAsyncEnumerable<BillActionType> billActionTypes, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var billActionTypeWriter = await BillActionTypeInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var billActionType in billActionTypes) {
            await nodeWriter.InsertAsync(billActionType);
            await searchableWriter.InsertAsync(billActionType);
            await nameableWriter.InsertAsync(billActionType);
            await billActionTypeWriter.InsertAsync(billActionType);
            await EntityCreator.WriteTerms(billActionType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in billActionType.TenantNodes) {
                tenantNode.NodeId = billActionType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
