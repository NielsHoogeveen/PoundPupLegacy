namespace PoundPupLegacy.CreateModel.Creators;

public class DenominationCreator : IEntityCreator<Denomination>
{
    public static async Task CreateAsync(IAsyncEnumerable<Denomination> denominations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var denominationWriter = await DenominationInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var denomination in denominations) {
            await nodeWriter.WriteAsync(denomination);
            await searchableWriter.WriteAsync(denomination);
            await nameableWriter.WriteAsync(denomination);
            await denominationWriter.WriteAsync(denomination);
            await EntityCreator.WriteTerms(denomination, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in denomination.TenantNodes) {
                tenantNode.NodeId = denomination.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
