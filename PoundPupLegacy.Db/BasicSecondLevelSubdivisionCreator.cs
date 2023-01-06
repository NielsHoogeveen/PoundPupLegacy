using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class BasicSecondLevelSubdivisionCreator : IEntityCreator<BasicSecondLevelSubdivision>
{
    public static async Task CreateAsync(IAsyncEnumerable<BasicSecondLevelSubdivision> subdivisions, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var geographicalEntityWriter = await GeographicalEnityWriter.CreateAsync(connection);
        await using var politicalEntityWriter = await PoliticalEntityWriter.CreateAsync(connection);
        await using var subdivisionWriter = await SubdivisionWriter.CreateAsync(connection);
        await using var isoCodedSubdivisionWriter = await ISOCodedSubdivisionWriter.CreateAsync(connection);
        await using var bottomLevelSubdivisionWriter = await BottomLevelSubdivisionWriter.CreateAsync(connection);
        await using var secondLevelSubdivisionWriter = await SecondLevelSubdivisionWriter.CreateAsync(connection);
        await using var basicSecondLevelSubdivisionWriter = await BasicSecondLevelSubdivisionWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);


        await foreach (var subdivision in subdivisions)
        {
            await nodeWriter.WriteAsync(subdivision);
            await documentableWriter.WriteAsync(subdivision);
            await nameableWriter.WriteAsync(subdivision);
            await geographicalEntityWriter.WriteAsync(subdivision);
            await politicalEntityWriter.WriteAsync(subdivision);
            await subdivisionWriter.WriteAsync(subdivision);
            await isoCodedSubdivisionWriter.WriteAsync(subdivision);
            await bottomLevelSubdivisionWriter.WriteAsync(subdivision);
            await secondLevelSubdivisionWriter.WriteAsync(subdivision);
            await basicSecondLevelSubdivisionWriter.WriteAsync(subdivision);
            await EntityCreator.WriteTerms(subdivision, termWriter, termReader, termHierarchyWriter);
        }
    }
}
