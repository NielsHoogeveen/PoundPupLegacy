namespace PoundPupLegacy.CreateModel.Creators;

public class HagueStatusCreator : IEntityCreator<HagueStatus>
{
    public static async Task CreateAsync(IAsyncEnumerable<HagueStatus> hagueStatuss, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var hagueStatusWriter = await HagueStatusInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var hagueStatus in hagueStatuss) {
            await nodeWriter.WriteAsync(hagueStatus);
            await searchableWriter.WriteAsync(hagueStatus);
            await nameableWriter.WriteAsync(hagueStatus);
            await hagueStatusWriter.WriteAsync(hagueStatus);
            await EntityCreator.WriteTerms(hagueStatus, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in hagueStatus.TenantNodes) {
                tenantNode.NodeId = hagueStatus.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
