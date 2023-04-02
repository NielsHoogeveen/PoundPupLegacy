namespace PoundPupLegacy.CreateModel.Creators;

public class ActCreator : IEntityCreator<Act>
{
    public async Task CreateAsync(IAsyncEnumerable<Act> acts, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var actWriter = await ActInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);

        await foreach (var act in acts) {
            await nodeWriter.InsertAsync(act);
            await searchableWriter.InsertAsync(act);
            await nameableWriter.InsertAsync(act);
            await documentableWriter.InsertAsync(act);
            await actWriter.InsertAsync(act);
            await EntityCreator.WriteTerms(act, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in act.TenantNodes) {
                tenantNode.NodeId = act.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
