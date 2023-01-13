using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class PersonCreator : IEntityCreator<Person>
{
    public static async Task CreateAsync(IAsyncEnumerable<Person> persons, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var partyWriter = await PartyWriter.CreateAsync(connection);
        await using var personWriter = await PersonWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var person in persons)
        {
            await nodeWriter.WriteAsync(person);
            await documentableWriter.WriteAsync(person);
            await locatableWriter.WriteAsync(person);
            await nameableWriter.WriteAsync(person);
            await partyWriter.WriteAsync(person);
            await personWriter.WriteAsync(person);
            await EntityCreator.WriteTerms(person, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in person.TenantNodes)
            {
                tenantNode.NodeId = person.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
