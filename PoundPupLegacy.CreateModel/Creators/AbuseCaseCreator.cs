namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AbuseCaseCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Locatable> locatableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Case> caseInserterFactory,
    IDatabaseInserterFactory<AbuseCase> abuseCaseInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<AbuseCase>
{

    public override async Task CreateAsync(IAsyncEnumerable<AbuseCase> abuseCases, IDbConnection connection)
    {

        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var caseWriter = await caseInserterFactory.CreateAsync(connection);
        await using var abuseCaseWriter = await abuseCaseInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var abuseCase in abuseCases) {
            await nodeWriter.InsertAsync(abuseCase);
            await searchableWriter.InsertAsync(abuseCase);
            await documentableWriter.InsertAsync(abuseCase);
            await locatableWriter.InsertAsync(abuseCase);
            await nameableWriter.InsertAsync(abuseCase);
            await caseWriter.InsertAsync(abuseCase);
            await abuseCaseWriter.InsertAsync(abuseCase);
            await WriteTerms(abuseCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in abuseCase.TenantNodes) {
                tenantNode.NodeId = abuseCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
