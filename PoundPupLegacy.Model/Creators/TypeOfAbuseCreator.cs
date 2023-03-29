namespace PoundPupLegacy.CreateModel.Creators;

public class TypeOfAbuseCreator : IEntityCreator<TypeOfAbuse>
{
    public static async Task CreateAsync(IAsyncEnumerable<TypeOfAbuse> typesOfAbuse, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var typeOfAbuseWriter = await TypeOfAbuseWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await (new TermReaderByNameFactory()).CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await (new VocabularyIdReaderByOwnerAndNameFactory()).CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var typeOfAbuse in typesOfAbuse) {
            await nodeWriter.WriteAsync(typeOfAbuse);
            await searchableWriter.WriteAsync(typeOfAbuse);
            await nameableWriter.WriteAsync(typeOfAbuse);
            await typeOfAbuseWriter.WriteAsync(typeOfAbuse);
            await EntityCreator.WriteTerms(typeOfAbuse, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in typeOfAbuse.TenantNodes) {
                tenantNode.NodeId = typeOfAbuse.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
