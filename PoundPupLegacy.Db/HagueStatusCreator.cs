using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class HagueStatusCreator : IEntityCreator<HagueStatus>
{
    public static async Task CreateAsync(IAsyncEnumerable<HagueStatus> hagueStatuss, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var hagueStatusWriter = await HagueStatusWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var hagueStatus in hagueStatuss)
        {
            await nodeWriter.WriteAsync(hagueStatus);
            await searchableWriter.WriteAsync(hagueStatus);
            await nameableWriter.WriteAsync(hagueStatus);
            await hagueStatusWriter.WriteAsync(hagueStatus);
            await EntityCreator.WriteTerms(hagueStatus, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in hagueStatus.TenantNodes)
            {
                tenantNode.NodeId = hagueStatus.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
