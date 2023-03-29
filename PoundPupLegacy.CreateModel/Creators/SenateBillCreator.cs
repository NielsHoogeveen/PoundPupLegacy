namespace PoundPupLegacy.CreateModel.Creators;

public class SenateBillCreator : IEntityCreator<SenateBill>
{
    public static async Task CreateAsync(IAsyncEnumerable<SenateBill> senateBills, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var billWriter = await BillInserter.CreateAsync(connection);
        await using var senateBillWriter = await SenateBillInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);

        await foreach (var senateBill in senateBills) {
            await nodeWriter.WriteAsync(senateBill);
            await searchableWriter.WriteAsync(senateBill);
            await nameableWriter.WriteAsync(senateBill);
            await documentableWriter.WriteAsync(senateBill);
            await billWriter.WriteAsync(senateBill);
            await senateBillWriter.WriteAsync(senateBill);
            await EntityCreator.WriteTerms(senateBill, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in senateBill.TenantNodes) {
                tenantNode.NodeId = senateBill.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
