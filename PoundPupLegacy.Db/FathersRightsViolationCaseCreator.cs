using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class FathersRightsViolationCaseCreator : IEntityCreator<FathersRightsViolationCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<FathersRightsViolationCase> fathersRightsViolationCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var caseWriter = await CaseWriter.CreateAsync(connection);
        await using var fathersRightsViolationCaseWriter = await FathersRightsViolationCaseWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        await foreach (var fathersRightsViolationCase in fathersRightsViolationCases)
        {
            await nodeWriter.WriteAsync(fathersRightsViolationCase);
            await documentableWriter.WriteAsync(fathersRightsViolationCase);
            await locatableWriter.WriteAsync(fathersRightsViolationCase);
            await caseWriter.WriteAsync(fathersRightsViolationCase);
            await fathersRightsViolationCaseWriter.WriteAsync(fathersRightsViolationCase);
            await EntityCreator.WriteTerms(fathersRightsViolationCase, termWriter, termReader, termHierarchyWriter);
        }
    }
}
