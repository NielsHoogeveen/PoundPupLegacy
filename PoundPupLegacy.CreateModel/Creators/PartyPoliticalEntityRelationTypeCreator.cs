namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PartyPoliticalEntityRelationTypeCreator : IEntityCreator<PartyPoliticalEntityRelationType>
{
    public async Task CreateAsync(IAsyncEnumerable<PartyPoliticalEntityRelationType> politicalEntityRelationTypes, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var politicalEntityRelationTypeWriter = await PoliticalEntityRelationTypeWriter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var politicalEntityRelationType in politicalEntityRelationTypes) {
            await nodeWriter.InsertAsync(politicalEntityRelationType);
            await searchableWriter.InsertAsync(politicalEntityRelationType);
            await nameableWriter.InsertAsync(politicalEntityRelationType);
            await politicalEntityRelationTypeWriter.InsertAsync(politicalEntityRelationType);
            await EntityCreator.WriteTerms(politicalEntityRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in politicalEntityRelationType.TenantNodes) {
                tenantNode.NodeId = politicalEntityRelationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
