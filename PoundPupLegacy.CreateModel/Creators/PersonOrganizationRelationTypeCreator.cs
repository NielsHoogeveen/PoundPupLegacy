namespace PoundPupLegacy.CreateModel.Creators;

public class PersonOrganizationRelationTypeCreator : IEntityCreator<PersonOrganizationRelationType>
{
    public async Task CreateAsync(IAsyncEnumerable<PersonOrganizationRelationType> personOrganizationRelationTypes, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var personOrganizationRelationTypeWriter = await PersonOrganizationRelationTypeInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var personOrganizationRelationType in personOrganizationRelationTypes) {
            await nodeWriter.InsertAsync(personOrganizationRelationType);
            await searchableWriter.InsertAsync(personOrganizationRelationType);
            await nameableWriter.InsertAsync(personOrganizationRelationType);
            await personOrganizationRelationTypeWriter.InsertAsync(personOrganizationRelationType);
            await EntityCreator.WriteTerms(personOrganizationRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in personOrganizationRelationType.TenantNodes) {
                tenantNode.NodeId = personOrganizationRelationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
