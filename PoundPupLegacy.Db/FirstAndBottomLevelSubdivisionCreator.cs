using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class FirstAndBottomLevelSubdivisionCreator : IEntityCreator<FirstAndBottomLevelSubdivision>
{
    public static async Task CreateAsync(IAsyncEnumerable<FirstAndBottomLevelSubdivision> subdivisions, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var geographicalEntityWriter = await GeographicalEnityWriter.CreateAsync(connection);
        await using var politicalEntityWriter = await PoliticalEntityWriter.CreateAsync(connection);
        await using var subdivisionWriter = await SubdivisionWriter.CreateAsync(connection);
        await using var isoCodedSubdivisionWriter = await ISOCodedSubdivisionWriter.CreateAsync(connection);
        await using var firstLevelSubdivisionWriter = await FirstLevelSubdivisionWriter.CreateAsync(connection);
        await using var isoCodedFirstLevelSubdivisionWriter = await ISOCodedFirstLevelSubdivisionWriter.CreateAsync(connection);
        await using var bottomLevelSubdivisionWriter = await BottomLevelSubdivisionWriter.CreateAsync(connection);
        await using var firstAndBottomLevelSubdivisionWriter = await FirstAndBottomLevelSubdivisionWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var subdivision in subdivisions)
        {
            await nodeWriter.WriteAsync(subdivision);
            await documentableWriter.WriteAsync(subdivision);
            await nameableWriter.WriteAsync(subdivision);
            await geographicalEntityWriter.WriteAsync(subdivision);
            await politicalEntityWriter.WriteAsync(subdivision);
            await subdivisionWriter.WriteAsync(subdivision);
            await bottomLevelSubdivisionWriter.WriteAsync(subdivision);
            await isoCodedSubdivisionWriter.WriteAsync(subdivision);
            await firstLevelSubdivisionWriter.WriteAsync(subdivision);
            await isoCodedFirstLevelSubdivisionWriter.WriteAsync(subdivision);
            await firstAndBottomLevelSubdivisionWriter.WriteAsync(subdivision);
            await EntityCreator.WriteTerms(subdivision, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in subdivision.TenantNodes)
            {
                tenantNode.NodeId = subdivision.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
