namespace PoundPupLegacy.CreateModel.Creators;

public class PersonCreator : IEntityCreator<Person>
{
    public async Task CreateAsync(IAsyncEnumerable<Person> persons, IDbConnection connection)
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
            await nodeWriter.InsertAsync(person);
            await searchableWriter.InsertAsync(person);
            await documentableWriter.InsertAsync(person);
            await locatableWriter.InsertAsync(person);
            await nameableWriter.InsertAsync(person);
            await partyWriter.InsertAsync(person);
            await personWriter.InsertAsync(person);
            await EntityCreator.WriteTerms(person, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in person.TenantNodes) {
                tenantNode.NodeId = person.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

            foreach (var role in person.ProfessionalRoles) {
                role.PersonId = person.Id;
            }
            await new ProfessionalRoleCreator().CreateAsync(person.ProfessionalRoles.ToAsyncEnumerable(), connection);

            foreach (var relation in person.PersonOrganizationRelations) {
                relation.PersonId = person.Id;
            }
            await new PersonOrganizationRelationCreator().CreateAsync(person.PersonOrganizationRelations.ToAsyncEnumerable(), connection);

        }
    }
}
