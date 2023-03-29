namespace PoundPupLegacy.CreateModel.Creators;

public class CoercedAdoptionCaseCreator : IEntityCreator<CoercedAdoptionCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<CoercedAdoptionCase> coercedAdoptionCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var locatableWriter = await LocatableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var caseWriter = await CaseInserter.CreateAsync(connection);
        await using var coercedAdoptionCaseWriter = await CoercedAdoptionCaseInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var coercedAdoptionCase in coercedAdoptionCases) {
            await nodeWriter.InsertAsync(coercedAdoptionCase);
            await searchableWriter.InsertAsync(coercedAdoptionCase);
            await documentableWriter.InsertAsync(coercedAdoptionCase);
            await locatableWriter.InsertAsync(coercedAdoptionCase);
            await nameableWriter.InsertAsync(coercedAdoptionCase);
            await caseWriter.InsertAsync(coercedAdoptionCase);
            await coercedAdoptionCaseWriter.InsertAsync(coercedAdoptionCase);
            await EntityCreator.WriteTerms(coercedAdoptionCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in coercedAdoptionCase.TenantNodes) {
                tenantNode.NodeId = coercedAdoptionCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
