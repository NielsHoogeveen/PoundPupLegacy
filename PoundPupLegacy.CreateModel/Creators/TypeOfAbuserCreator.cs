namespace PoundPupLegacy.CreateModel.Creators;

public class TypeOfAbuserCreator : IEntityCreator<TypeOfAbuser>
{
    public static async Task CreateAsync(IAsyncEnumerable<TypeOfAbuser> typesOfAbuser, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var typeOfAbuserWriter = await TypeOfAbuserWriter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var typeOfAbuser in typesOfAbuser) {
            await nodeWriter.InsertAsync(typeOfAbuser);
            await searchableWriter.InsertAsync(typeOfAbuser);
            await nameableWriter.InsertAsync(typeOfAbuser);
            await typeOfAbuserWriter.InsertAsync(typeOfAbuser);
            await EntityCreator.WriteTerms(typeOfAbuser, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in typeOfAbuser.TenantNodes) {
                tenantNode.NodeId = typeOfAbuser.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
