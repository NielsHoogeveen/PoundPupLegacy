namespace PoundPupLegacy.CreateModel.Creators;

public class OrganizationTypeCreator : IEntityCreator<OrganizationType>
{
    public static async Task CreateAsync(IAsyncEnumerable<OrganizationType> organizationTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var organizationTypeWriter = await OrganizationTypeInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var organizationType in organizationTypes) {
            await nodeWriter.WriteAsync(organizationType);
            await searchableWriter.WriteAsync(organizationType);
            await nameableWriter.WriteAsync(organizationType);
            await organizationTypeWriter.WriteAsync(organizationType);
            await EntityCreator.WriteTerms(organizationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in organizationType.TenantNodes) {
                tenantNode.NodeId = organizationType.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
