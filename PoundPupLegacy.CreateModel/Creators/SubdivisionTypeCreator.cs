namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SubdivisionTypeCreator : IEntityCreator<SubdivisionType>
{
    public async Task CreateAsync(IAsyncEnumerable<SubdivisionType> subdivisionTypes, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var subdivisionTypeWriter = await SubdivisionTypeInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var subdivisionType in subdivisionTypes) {
            await nodeWriter.InsertAsync(subdivisionType);
            await searchableWriter.InsertAsync(subdivisionType);
            await nameableWriter.InsertAsync(subdivisionType);
            await subdivisionTypeWriter.InsertAsync(subdivisionType);
            await EntityCreator.WriteTerms(subdivisionType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in subdivisionType.TenantNodes) {
                tenantNode.NodeId = subdivisionType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
