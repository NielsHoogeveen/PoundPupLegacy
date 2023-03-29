namespace PoundPupLegacy.CreateModel.Creators;

public class SubdivisionTypeCreator : IEntityCreator<SubdivisionType>
{
    public static async Task CreateAsync(IAsyncEnumerable<SubdivisionType> subdivisionTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var subdivisionTypeWriter = await SubdivisionTypeInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var subdivisionType in subdivisionTypes) {
            await nodeWriter.WriteAsync(subdivisionType);
            await searchableWriter.WriteAsync(subdivisionType);
            await nameableWriter.WriteAsync(subdivisionType);
            await subdivisionTypeWriter.WriteAsync(subdivisionType);
            await EntityCreator.WriteTerms(subdivisionType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in subdivisionType.TenantNodes) {
                tenantNode.NodeId = subdivisionType.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
