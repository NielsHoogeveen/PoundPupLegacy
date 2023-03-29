namespace PoundPupLegacy.CreateModel.Creators;

public class PartyPoliticalEntityRelationTypeCreator : IEntityCreator<PartyPoliticalEntityRelationType>
{
    public static async Task CreateAsync(IAsyncEnumerable<PartyPoliticalEntityRelationType> politicalEntityRelationTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var politicalEntityRelationTypeWriter = await PoliticalEntityRelationTypeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var politicalEntityRelationType in politicalEntityRelationTypes) {
            await nodeWriter.WriteAsync(politicalEntityRelationType);
            await searchableWriter.WriteAsync(politicalEntityRelationType);
            await nameableWriter.WriteAsync(politicalEntityRelationType);
            await politicalEntityRelationTypeWriter.WriteAsync(politicalEntityRelationType);
            await EntityCreator.WriteTerms(politicalEntityRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in politicalEntityRelationType.TenantNodes) {
                tenantNode.NodeId = politicalEntityRelationType.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
