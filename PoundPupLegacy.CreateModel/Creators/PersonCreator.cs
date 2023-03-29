namespace PoundPupLegacy.CreateModel.Creators;

public class PersonCreator : IEntityCreator<Person>
{
    public static async Task CreateAsync(IAsyncEnumerable<Person> persons, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var locatableWriter = await LocatableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var partyWriter = await PartyInserter.CreateAsync(connection);
        await using var personWriter = await PersonInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var person in persons) {
            await nodeWriter.WriteAsync(person);
            await searchableWriter.WriteAsync(person);
            await documentableWriter.WriteAsync(person);
            await locatableWriter.WriteAsync(person);
            await nameableWriter.WriteAsync(person);
            await partyWriter.WriteAsync(person);
            await personWriter.WriteAsync(person);
            await EntityCreator.WriteTerms(person, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in person.TenantNodes) {
                tenantNode.NodeId = person.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

            foreach (var role in person.ProfessionalRoles) {
                role.PersonId = person.Id;
            }
            await ProfessionalRoleCreator.CreateAsync(person.ProfessionalRoles.ToAsyncEnumerable(), connection);

            foreach (var relation in person.PersonOrganizationRelations) {
                relation.PersonId = person.Id;
            }
            await PersonOrganizationRelationCreator.CreateAsync(person.PersonOrganizationRelations.ToAsyncEnumerable(), connection);

        }
    }
}
