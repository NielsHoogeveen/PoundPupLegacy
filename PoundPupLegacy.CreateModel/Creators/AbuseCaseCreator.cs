namespace PoundPupLegacy.CreateModel.Creators;

public class AbuseCaseCreator : IEntityCreator<AbuseCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<AbuseCase> abuseCases, NpgsqlConnection connection)
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
            await nodeWriter.WriteAsync(abuseCase);
            await searchableWriter.WriteAsync(abuseCase);
            await documentableWriter.WriteAsync(abuseCase);
            await locatableWriter.WriteAsync(abuseCase);
            await nameableWriter.WriteAsync(abuseCase);
            await caseWriter.WriteAsync(abuseCase);
            await abuseCaseWriter.WriteAsync(abuseCase);
            await EntityCreator.WriteTerms(abuseCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in abuseCase.TenantNodes) {
                tenantNode.NodeId = abuseCase.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
