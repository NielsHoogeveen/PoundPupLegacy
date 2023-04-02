namespace PoundPupLegacy.CreateModel.Creators;

public class DeportationCaseCreator : IEntityCreator<DeportationCase>
{
    public async Task CreateAsync(IAsyncEnumerable<DeportationCase> deportationCases, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var locatableWriter = await LocatableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var caseWriter = await CaseInserter.CreateAsync(connection);
        await using var deportationCaseWriter = await DeportationCaseInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);


        await foreach (var deportationCase in deportationCases) {
            await nodeWriter.InsertAsync(deportationCase);
            await searchableWriter.InsertAsync(deportationCase);
            await documentableWriter.InsertAsync(deportationCase);
            await locatableWriter.InsertAsync(deportationCase);
            await nameableWriter.InsertAsync(deportationCase);
            await caseWriter.InsertAsync(deportationCase);
            await deportationCaseWriter.InsertAsync(deportationCase);
            await EntityCreator.WriteTerms(deportationCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in deportationCase.TenantNodes) {
                tenantNode.NodeId = deportationCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
