namespace PoundPupLegacy.CreateModel.Creators;

public class ProfessionCreator : IEntityCreator<Profession>
{
    public static async Task CreateAsync(IAsyncEnumerable<Profession> professions, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var professionWriter = await ProfessionInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var profession in professions) {
            await nodeWriter.WriteAsync(profession);
            await searchableWriter.WriteAsync(profession);
            await nameableWriter.WriteAsync(profession);
            await professionWriter.WriteAsync(profession);
            await EntityCreator.WriteTerms(profession, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in profession.TenantNodes) {
                tenantNode.NodeId = profession.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
