namespace PoundPupLegacy.CreateModel.Creators;

public class FamilySizeCreator : IEntityCreator<FamilySize>
{
    public static async Task CreateAsync(IAsyncEnumerable<FamilySize> familySizes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var familySizeWriter = await FamilySizeInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var familySize in familySizes) {
            await nodeWriter.WriteAsync(familySize);
            await searchableWriter.WriteAsync(familySize);
            await nameableWriter.WriteAsync(familySize);
            await familySizeWriter.WriteAsync(familySize);
            await EntityCreator.WriteTerms(familySize, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in familySize.TenantNodes) {
                tenantNode.NodeId = familySize.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
