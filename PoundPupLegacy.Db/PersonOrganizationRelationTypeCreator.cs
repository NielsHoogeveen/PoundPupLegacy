using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class PersonOrganizationRelationTypeCreator : IEntityCreator<PersonOrganizationRelationType>
{
    public static async Task CreateAsync(IAsyncEnumerable<PersonOrganizationRelationType> personOrganizationRelationTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var personOrganizationRelationTypeWriter = await PersonOrganizationRelationTypeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var personOrganizationRelationType in personOrganizationRelationTypes)
        {
            await nodeWriter.WriteAsync(personOrganizationRelationType);
            await nameableWriter.WriteAsync(personOrganizationRelationType);
            await personOrganizationRelationTypeWriter.WriteAsync(personOrganizationRelationType);
            await EntityCreator.WriteTerms(personOrganizationRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in personOrganizationRelationType.TenantNodes)
            {
                tenantNode.NodeId = personOrganizationRelationType.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
