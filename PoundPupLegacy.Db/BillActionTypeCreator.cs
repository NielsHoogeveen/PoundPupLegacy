using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class BillActionTypeCreator : IEntityCreator<BillActionType>
{
    public static async Task CreateAsync(IAsyncEnumerable<BillActionType> billActionTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var billActionTypeWriter = await BillActionTypeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await (new TermReaderByNameFactory()).CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await ( new VocabularyIdReaderByOwnerAndNameFactory()).CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var billActionType in billActionTypes) {
            await nodeWriter.WriteAsync(billActionType);
            await searchableWriter.WriteAsync(billActionType);
            await nameableWriter.WriteAsync(billActionType);
            await billActionTypeWriter.WriteAsync(billActionType);
            await EntityCreator.WriteTerms(billActionType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in billActionType.TenantNodes) {
                tenantNode.NodeId = billActionType.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
