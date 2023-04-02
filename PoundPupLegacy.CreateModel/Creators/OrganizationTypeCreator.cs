namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class OrganizationTypeCreator : IEntityCreator<OrganizationType>
{
    public async Task CreateAsync(IAsyncEnumerable<OrganizationType> organizationTypes, IDbConnection connection)
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
            await nodeWriter.InsertAsync(organizationType);
            await searchableWriter.InsertAsync(organizationType);
            await nameableWriter.InsertAsync(organizationType);
            await organizationTypeWriter.InsertAsync(organizationType);
            await EntityCreator.WriteTerms(organizationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in organizationType.TenantNodes) {
                tenantNode.NodeId = organizationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
