namespace PoundPupLegacy.CreateModel.Creators;

public class AbuseCaseCreator : IEntityCreator<AbuseCase>
{
    public async Task CreateAsync(IAsyncEnumerable<AbuseCase> abuseCases, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var locatableWriter = await LocatableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var caseWriter = await CaseInserter.CreateAsync(connection);
        await using var abuseCaseWriter = await AbuseCaseInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var abuseCase in abuseCases) {
            await nodeWriter.InsertAsync(abuseCase);
            await searchableWriter.InsertAsync(abuseCase);
            await documentableWriter.InsertAsync(abuseCase);
            await locatableWriter.InsertAsync(abuseCase);
            await nameableWriter.InsertAsync(abuseCase);
            await caseWriter.InsertAsync(abuseCase);
            await abuseCaseWriter.InsertAsync(abuseCase);
            await EntityCreator.WriteTerms(abuseCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in abuseCase.TenantNodes) {
                tenantNode.NodeId = abuseCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
