using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class TypeOfAbuserCreator : IEntityCreator<TypeOfAbuser>
{
    public static async Task CreateAsync(IAsyncEnumerable<TypeOfAbuser> typesOfAbuser, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var typeOfAbuserWriter = await TypeOfAbuserWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var typeOfAbuser in typesOfAbuser)
        {
            await nodeWriter.WriteAsync(typeOfAbuser);
            await nameableWriter.WriteAsync(typeOfAbuser);
            await typeOfAbuserWriter.WriteAsync(typeOfAbuser);
            await EntityCreator.WriteTerms(typeOfAbuser, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in typeOfAbuser.TenantNodes)
            {
                tenantNode.NodeId = typeOfAbuser.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
