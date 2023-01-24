using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class SubdivisionTypeCreator : IEntityCreator<SubdivisionType>
{
    public static async Task CreateAsync(IAsyncEnumerable<SubdivisionType> subdivisionTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var subdivisionTypeWriter = await SubdivisionTypeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var subdivisionType in subdivisionTypes)
        {
            await nodeWriter.WriteAsync(subdivisionType);
            await nameableWriter.WriteAsync(subdivisionType);
            await subdivisionTypeWriter.WriteAsync(subdivisionType);
            await EntityCreator.WriteTerms(subdivisionType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in subdivisionType.TenantNodes)
            {
                tenantNode.NodeId = subdivisionType.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
