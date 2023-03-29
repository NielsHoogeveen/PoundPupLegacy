namespace PoundPupLegacy.CreateModel.Creators;

public class FathersRightsViolationCaseCreator : IEntityCreator<FathersRightsViolationCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<FathersRightsViolationCase> fathersRightsViolationCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var locatableWriter = await LocatableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var caseWriter = await CaseInserter.CreateAsync(connection);
        await using var fathersRightsViolationCaseWriter = await FathersRightsViolationCaseWriter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var fathersRightsViolationCase in fathersRightsViolationCases) {
            await nodeWriter.InsertAsync(fathersRightsViolationCase);
            await searchableWriter.InsertAsync(fathersRightsViolationCase);
            await documentableWriter.InsertAsync(fathersRightsViolationCase);
            await locatableWriter.InsertAsync(fathersRightsViolationCase);
            await nameableWriter.InsertAsync(fathersRightsViolationCase);
            await caseWriter.InsertAsync(fathersRightsViolationCase);
            await fathersRightsViolationCaseWriter.InsertAsync(fathersRightsViolationCase);
            await EntityCreator.WriteTerms(fathersRightsViolationCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in fathersRightsViolationCase.TenantNodes) {
                tenantNode.NodeId = fathersRightsViolationCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
