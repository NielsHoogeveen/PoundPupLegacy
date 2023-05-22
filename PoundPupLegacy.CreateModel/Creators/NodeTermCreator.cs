namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeTermCreator(
    IDatabaseInserterFactory<NodeTerm> nodeTermInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Locatable> locatableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Case> caseInserterFactory,
    IDatabaseInserterFactory<NewAbuseCase> abuseCaseInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory
) : EntityCreator<NodeTerm>
{
    public override async Task CreateAsync(IAsyncEnumerable<NodeTerm> nodeTerms, IDbConnection connection)
    {
        await using var nodeTermWriter = await nodeTermInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var caseWriter = await caseInserterFactory.CreateAsync(connection);
        await using var abuseCaseWriter = await abuseCaseInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);

        await foreach (var nodeTerm in nodeTerms) {
            await nodeTermWriter.InsertAsync(nodeTerm);
        }
    }
}
