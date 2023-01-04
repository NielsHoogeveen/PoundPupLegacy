using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class AbuseCaseCreator : IEntityCreator<AbuseCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<AbuseCase> abuseCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var caseWriter = await CaseWriter.CreateAsync(connection);
        await using var abuseCaseWriter = await AbuseCaseWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        await foreach (var abuseCase in abuseCases)
        {
            await nodeWriter.WriteAsync(abuseCase);
            await documentableWriter.WriteAsync(abuseCase);
            await locatableWriter.WriteAsync(abuseCase);
            await caseWriter.WriteAsync(abuseCase);
            await abuseCaseWriter.WriteAsync(abuseCase);
            await EntityCreator.WriteTerms(abuseCase, termWriter, termReader, termHierarchyWriter);
        }
    }
}
