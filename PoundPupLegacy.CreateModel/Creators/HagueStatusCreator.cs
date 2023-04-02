namespace PoundPupLegacy.CreateModel.Creators;

public class HagueStatusCreator : IEntityCreator<HagueStatus>
{
    public async Task CreateAsync(IAsyncEnumerable<HagueStatus> hagueStatuss, IDbConnection connection)
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
            await nodeWriter.InsertAsync(hagueStatus);
            await searchableWriter.InsertAsync(hagueStatus);
            await nameableWriter.InsertAsync(hagueStatus);
            await hagueStatusWriter.InsertAsync(hagueStatus);
            await EntityCreator.WriteTerms(hagueStatus, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in hagueStatus.TenantNodes) {
                tenantNode.NodeId = hagueStatus.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
