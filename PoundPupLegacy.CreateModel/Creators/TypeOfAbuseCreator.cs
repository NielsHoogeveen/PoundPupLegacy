namespace PoundPupLegacy.CreateModel.Creators;

public class TypeOfAbuseCreator : IEntityCreator<TypeOfAbuse>
{
    public async Task CreateAsync(IAsyncEnumerable<TypeOfAbuse> typesOfAbuse, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var typeOfAbuseWriter = await TypeOfAbuseWriter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var typeOfAbuse in typesOfAbuse) {
            await nodeWriter.InsertAsync(typeOfAbuse);
            await searchableWriter.InsertAsync(typeOfAbuse);
            await nameableWriter.InsertAsync(typeOfAbuse);
            await typeOfAbuseWriter.InsertAsync(typeOfAbuse);
            await EntityCreator.WriteTerms(typeOfAbuse, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in typeOfAbuse.TenantNodes) {
                tenantNode.NodeId = typeOfAbuse.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
