using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class AbuseCaseCreator : IEntityCreator<AbuseCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<AbuseCase> abuseCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var caseWriter = await CaseWriter.CreateAsync(connection);
        await using var abuseCaseWriter = await AbuseCaseWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var abuseCase in abuseCases)
        {
            await nodeWriter.WriteAsync(abuseCase);
            await searchableWriter.WriteAsync(abuseCase);
            await documentableWriter.WriteAsync(abuseCase);
            await locatableWriter.WriteAsync(abuseCase);
            await nameableWriter.WriteAsync(abuseCase);
            await caseWriter.WriteAsync(abuseCase);
            await abuseCaseWriter.WriteAsync(abuseCase);
            await EntityCreator.WriteTerms(abuseCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in abuseCase.TenantNodes)
            {
                tenantNode.NodeId = abuseCase.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
