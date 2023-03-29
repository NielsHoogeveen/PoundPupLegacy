namespace PoundPupLegacy.CreateModel.Creators;

public class BasicNameableCreator : IEntityCreator<BasicNameable>
{
    public static async Task CreateAsync(IAsyncEnumerable<BasicNameable> basicNameables, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var basicNameableWriter = await BasicNameableInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);

        await foreach (var basicNameable in basicNameables) {
            await nodeWriter.InsertAsync(basicNameable);
            await searchableWriter.InsertAsync(basicNameable);
            await nameableWriter.InsertAsync(basicNameable);
            await basicNameableWriter.InsertAsync(basicNameable);
            await EntityCreator.WriteTerms(basicNameable, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in basicNameable.TenantNodes) {
                tenantNode.NodeId = basicNameable.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
