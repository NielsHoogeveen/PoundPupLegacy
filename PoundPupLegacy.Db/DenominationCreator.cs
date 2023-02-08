using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class DenominationCreator : IEntityCreator<Denomination>
{
    public static async Task CreateAsync(IAsyncEnumerable<Denomination> denominations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var denominationWriter = await DenominationWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var denomination in denominations)
        {
            await nodeWriter.WriteAsync(denomination);
            await searchableWriter.WriteAsync(denomination);
            await nameableWriter.WriteAsync(denomination);
            await denominationWriter.WriteAsync(denomination);
            await EntityCreator.WriteTerms(denomination, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in denomination.TenantNodes)
            {
                tenantNode.NodeId = denomination.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
