using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class DeportationCaseCreator : IEntityCreator<DeportationCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<DeportationCase> deportationCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var caseWriter = await CaseWriter.CreateAsync(connection);
        await using var deportationCaseWriter = await DeportationCaseWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        await foreach (var deportationCase in deportationCases)
        {
            await nodeWriter.WriteAsync(deportationCase);
            await documentableWriter.WriteAsync(deportationCase);
            await locatableWriter.WriteAsync(deportationCase);
            await caseWriter.WriteAsync(deportationCase);
            await deportationCaseWriter.WriteAsync(deportationCase);
            await EntityCreator.WriteTerms(deportationCase, termWriter, termReader, termHierarchyWriter);
        }
    }
}
