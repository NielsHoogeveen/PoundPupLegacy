using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class InformalIntermediateLevelSubdivisionCreator : IEntityCreator<InformalIntermediateLevelSubdivision>
{
    public static async Task CreateAsync(IAsyncEnumerable<InformalIntermediateLevelSubdivision> subdivisions, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var geographicalEntityWriter = await GeographicalEnityWriter.CreateAsync(connection);
        await using var subdivisionWriter = await SubdivisionWriter.CreateAsync(connection);
        await using var firstLevelSubdivisionWriter = await FirstLevelSubdivisionWriter.CreateAsync(connection);
        await using var intermediateLevelSubdivisionWriter = await IntermediateLevelSubdivisionWriter.CreateAsync(connection);
        await using var formalIntermediateLevelSubdivisionWriter = await InformalIntermediateLevelSubdivisionWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        await foreach (var subdivision in subdivisions)
        {
            await nodeWriter.WriteAsync(subdivision);
            await documentableWriter.WriteAsync(subdivision);
            await geographicalEntityWriter.WriteAsync(subdivision);
            await subdivisionWriter.WriteAsync(subdivision);
            await firstLevelSubdivisionWriter.WriteAsync(subdivision);
            await intermediateLevelSubdivisionWriter.WriteAsync(subdivision);
            await formalIntermediateLevelSubdivisionWriter.WriteAsync(subdivision);
            await EntityCreator.WriteTerms(subdivision, termWriter, termReader, termHierarchyWriter);
        }
    }
}
