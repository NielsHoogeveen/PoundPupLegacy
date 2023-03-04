using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class NodeTermCreator : IEntityCreator<NodeTerm>
{
    public static async Task CreateAsync(IAsyncEnumerable<NodeTerm> nodeTerms, NpgsqlConnection connection)
    {

        await using var nodeTermWriter = await NodeTermWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var caseWriter = await CaseWriter.CreateAsync(connection);
        await using var abuseCaseWriter = await AbuseCaseWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        await foreach (var nodeTerm in nodeTerms) {
            await nodeTermWriter.WriteAsync(nodeTerm);
        }
    }
}
