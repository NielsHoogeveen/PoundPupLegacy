namespace PoundPupLegacy.CreateModel.Creators;

public class FamilySizeCreator : IEntityCreator<FamilySize>
{
    public static async Task CreateAsync(IAsyncEnumerable<FamilySize> familySizes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var familySizeWriter = await FamilySizeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

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
