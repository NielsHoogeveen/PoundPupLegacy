using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class PoliticalEntityRelationTypeCreator : IEntityCreator<PoliticalEntityRelationType>
{
    public static async Task CreateAsync(IAsyncEnumerable<PoliticalEntityRelationType> politicalEntityRelationTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var politicalEntityRelationTypeWriter = await PoliticalEntityRelationTypeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        await foreach (var politicalEntityRelationType in politicalEntityRelationTypes)
        {
            await nodeWriter.WriteAsync(politicalEntityRelationType);
            await nameableWriter.WriteAsync(politicalEntityRelationType);
            await politicalEntityRelationTypeWriter.WriteAsync(politicalEntityRelationType);
            await EntityCreator.WriteTerms(politicalEntityRelationType, termWriter, termReader, termHierarchyWriter);
        }
    }
}
