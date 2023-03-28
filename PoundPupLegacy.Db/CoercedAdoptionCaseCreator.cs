using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class CoercedAdoptionCaseCreator : IEntityCreator<CoercedAdoptionCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<CoercedAdoptionCase> coercedAdoptionCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var caseWriter = await CaseWriter.CreateAsync(connection);
        await using var coercedAdoptionCaseWriter = await CoercedAdoptionCaseWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await (new TermReaderByNameFactory()).CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await ( new VocabularyIdReaderByOwnerAndNameFactory()).CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var coercedAdoptionCase in coercedAdoptionCases) {
            await nodeWriter.WriteAsync(coercedAdoptionCase);
            await searchableWriter.WriteAsync(coercedAdoptionCase);
            await documentableWriter.WriteAsync(coercedAdoptionCase);
            await locatableWriter.WriteAsync(coercedAdoptionCase);
            await nameableWriter.WriteAsync(coercedAdoptionCase);
            await caseWriter.WriteAsync(coercedAdoptionCase);
            await coercedAdoptionCaseWriter.WriteAsync(coercedAdoptionCase);
            await EntityCreator.WriteTerms(coercedAdoptionCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in coercedAdoptionCase.TenantNodes) {
                tenantNode.NodeId = coercedAdoptionCase.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
