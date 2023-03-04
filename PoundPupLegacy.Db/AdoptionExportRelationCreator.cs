namespace PoundPupLegacy.Db;

public class AdoptionExportRelationCreator : IEntityCreator<AdoptionExportRelation>
{
    public static async Task CreateAsync(IAsyncEnumerable<AdoptionExportRelation> nodeTypes, NpgsqlConnection connection)
    {

        await using var nodeTypeWriter = await AdoptionExportRelationWriter.CreateAsync(connection);

        await foreach (var nodeType in nodeTypes) {
            await nodeTypeWriter.WriteAsync(nodeType);
        }
    }
}
