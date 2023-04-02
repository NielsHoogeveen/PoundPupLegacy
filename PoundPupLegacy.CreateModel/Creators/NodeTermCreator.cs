namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeTermCreator : IEntityCreator<NodeTerm>
{
    public async Task CreateAsync(IAsyncEnumerable<NodeTerm> nodeTerms, IDbConnection connection)
    {

        await using var nodeTermWriter = await NodeTermInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var locatableWriter = await LocatableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var caseWriter = await CaseInserter.CreateAsync(connection);
        await using var abuseCaseWriter = await AbuseCaseInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);

        await foreach (var nodeTerm in nodeTerms) {
            await nodeTermWriter.InsertAsync(nodeTerm);
        }
    }
}
