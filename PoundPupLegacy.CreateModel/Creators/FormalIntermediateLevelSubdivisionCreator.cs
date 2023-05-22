namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FormalIntermediateLevelSubdivisionCreator(
    IDatabaseInserterFactory<NewFormalIntermediateLevelSubdivision> formalIntermediateLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<IntermediateLevelSubdivision> intermediateLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<ISOCodedFirstLevelSubdivision> isoCodedFirstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<FirstLevelSubdivision> firstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<ISOCodedSubdivision> isoCodedSubdivisionInserterFactory,
    IDatabaseInserterFactory<Subdivision> subdivisionInserterFactory,
    IDatabaseInserterFactory<PoliticalEntity> politicalEntityInserterFactory,
    IDatabaseInserterFactory<GeographicalEntity> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewFormalIntermediateLevelSubdivision>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewFormalIntermediateLevelSubdivision> subdivisions, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var geographicalEntityWriter = await geographicalEntityInserterFactory.CreateAsync(connection);
        await using var politicalEntityWriter = await politicalEntityInserterFactory.CreateAsync(connection);
        await using var subdivisionWriter = await subdivisionInserterFactory.CreateAsync(connection);
        await using var isoCodedSubdivisionWriter = await isoCodedSubdivisionInserterFactory.CreateAsync(connection);
        await using var firstLevelSubdivisionWriter = await firstLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var isoCodedFirstLevelSubdivisionWriter = await isoCodedFirstLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var intermediateLevelSubdivisionWriter = await intermediateLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var formalIntermediateLevelSubdivisionWriter = await formalIntermediateLevelSubdivisionInserterFactory.CreateAsync(connection);
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
            await firstLevelSubdivisionWriter.InsertAsync(subdivision);
            await intermediateLevelSubdivisionWriter.InsertAsync(subdivision);
            await isoCodedFirstLevelSubdivisionWriter.InsertAsync(subdivision);
            await formalIntermediateLevelSubdivisionWriter.InsertAsync(subdivision);
            await WriteTerms(subdivision, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in subdivision.TenantNodes) {
                tenantNode.NodeId = subdivision.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
