namespace PoundPupLegacy.CreateModel.Creators;

public class InterOrganizationalRelationTypeCreator : IEntityCreator<InterOrganizationalRelationType>
{
    public async Task CreateAsync(IAsyncEnumerable<InterOrganizationalRelationType> interOrganizationalRelationTypes, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var interOrganizationalRelationTypeWriter = await InterOrganizationalRelationTypeInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var interOrganizationalRelationType in interOrganizationalRelationTypes) {
            await nodeWriter.InsertAsync(interOrganizationalRelationType);
            await searchableWriter.InsertAsync(interOrganizationalRelationType);
            await nameableWriter.InsertAsync(interOrganizationalRelationType);
            await interOrganizationalRelationTypeWriter.InsertAsync(interOrganizationalRelationType);
            await EntityCreator.WriteTerms(interOrganizationalRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in interOrganizationalRelationType.TenantNodes) {
                tenantNode.NodeId = interOrganizationalRelationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
