namespace PoundPupLegacy.CreateModel.Creators;

public class InterPersonalRelationTypeCreator : IEntityCreator<InterPersonalRelationType>
{
    public async Task CreateAsync(IAsyncEnumerable<InterPersonalRelationType> interPersonalRelationTypes, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var interPersonalRelationTypeWriter = await InterPersonalRelationTypeInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var interPersonalRelationType in interPersonalRelationTypes) {
            await nodeWriter.InsertAsync(interPersonalRelationType);
            await searchableWriter.InsertAsync(interPersonalRelationType);
            await nameableWriter.InsertAsync(interPersonalRelationType);
            await interPersonalRelationTypeWriter.InsertAsync(interPersonalRelationType);
            await EntityCreator.WriteTerms(interPersonalRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in interPersonalRelationType.TenantNodes) {
                tenantNode.NodeId = interPersonalRelationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
