namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicSecondLevelSubdivisionCreator : IEntityCreator<BasicSecondLevelSubdivision>
{
    public async Task CreateAsync(IAsyncEnumerable<BasicSecondLevelSubdivision> subdivisions, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var geographicalEntityWriter = await GeographicalEnityInserter.CreateAsync(connection);
        await using var politicalEntityWriter = await PoliticalEntityInserter.CreateAsync(connection);
        await using var subdivisionWriter = await SubdivisionInserter.CreateAsync(connection);
        await using var isoCodedSubdivisionWriter = await ISOCodedSubdivisionInserter.CreateAsync(connection);
        await using var bottomLevelSubdivisionWriter = await BottomLevelSubdivisionInserter.CreateAsync(connection);
        await using var secondLevelSubdivisionWriter = await SecondLevelSubdivisionInserter.CreateAsync(connection);
        await using var basicSecondLevelSubdivisionWriter = await BasicSecondLevelSubdivisionInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var subdivision in subdivisions) {
            await nodeWriter.InsertAsync(subdivision);
            await searchableWriter.InsertAsync(subdivision);
            await documentableWriter.InsertAsync(subdivision);
            await nameableWriter.InsertAsync(subdivision);
            await geographicalEntityWriter.InsertAsync(subdivision);
            await politicalEntityWriter.InsertAsync(subdivision);
            await subdivisionWriter.InsertAsync(subdivision);
            await isoCodedSubdivisionWriter.InsertAsync(subdivision);
            await bottomLevelSubdivisionWriter.InsertAsync(subdivision);
            await secondLevelSubdivisionWriter.InsertAsync(subdivision);
            await basicSecondLevelSubdivisionWriter.InsertAsync(subdivision);
            await EntityCreator.WriteTerms(subdivision, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in subdivision.TenantNodes) {
                tenantNode.NodeId = subdivision.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
