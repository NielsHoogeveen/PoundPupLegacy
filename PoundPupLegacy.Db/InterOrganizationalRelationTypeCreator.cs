using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class InterOrganizationalRelationTypeCreator : IEntityCreator<InterOrganizationalRelationType>
{
    public static async Task CreateAsync(IAsyncEnumerable<InterOrganizationalRelationType> interOrganizationalRelationTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var interOrganizationalRelationTypeWriter = await InterOrganizationalRelationTypeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var interOrganizationalRelationType in interOrganizationalRelationTypes)
        {
            await nodeWriter.WriteAsync(interOrganizationalRelationType);
            await searchableWriter.WriteAsync(interOrganizationalRelationType);
            await nameableWriter.WriteAsync(interOrganizationalRelationType);
            await interOrganizationalRelationTypeWriter.WriteAsync(interOrganizationalRelationType);
            await EntityCreator.WriteTerms(interOrganizationalRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in interOrganizationalRelationType.TenantNodes)
            {
                tenantNode.NodeId = interOrganizationalRelationType.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
