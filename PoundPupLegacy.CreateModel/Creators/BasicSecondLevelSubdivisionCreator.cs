namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicSecondLevelSubdivisionCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<GeographicalEntity> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<PoliticalEntity> politicalEntityInserterFactory,
    IDatabaseInserterFactory<Subdivision> subdivisionInserterFactory,
    IDatabaseInserterFactory<ISOCodedSubdivision> isoCodedSubdivisionInserterFactory,
    IDatabaseInserterFactory<BottomLevelSubdivision> bottomLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<SecondLevelSubdivision> secondLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<BasicSecondLevelSubdivision> basicSecondLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<BasicSecondLevelSubdivision>
{
    public override async Task CreateAsync(IAsyncEnumerable<BasicSecondLevelSubdivision> subdivisions, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var geographicalEntityWriter = await geographicalEntityInserterFactory.CreateAsync(connection);
        await using var politicalEntityWriter = await politicalEntityInserterFactory.CreateAsync(connection);
        await using var subdivisionWriter = await subdivisionInserterFactory.CreateAsync(connection);
        await using var isoCodedSubdivisionWriter = await isoCodedSubdivisionInserterFactory.CreateAsync(connection);
        await using var bottomLevelSubdivisionWriter = await bottomLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var secondLevelSubdivisionWriter = await secondLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var basicSecondLevelSubdivisionWriter = await basicSecondLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

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
            await WriteTerms(subdivision, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in subdivision.TenantNodes) {
                tenantNode.NodeId = subdivision.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
