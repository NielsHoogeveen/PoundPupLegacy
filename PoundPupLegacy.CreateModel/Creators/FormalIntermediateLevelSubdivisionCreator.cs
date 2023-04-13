namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FormalIntermediateLevelSubdivisionCreator : EntityCreator<FormalIntermediateLevelSubdivision>
{
    private readonly IDatabaseInserterFactory<FormalIntermediateLevelSubdivision> _formalIntermediateLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<IntermediateLevelSubdivision> _intermediateLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<ISOCodedFirstLevelSubdivision> _isoCodedFirstLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<FirstLevelSubdivision> _firstLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<ISOCodedSubdivision> _isoCodedSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<Subdivision> _subdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<PoliticalEntity> _politicalEntityInserterFactory;
    private readonly IDatabaseInserterFactory<GeographicalEntity> _geographicalEntityInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> _termReaderFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;

    //Add constructor
    public FormalIntermediateLevelSubdivisionCreator(
        IDatabaseInserterFactory<FormalIntermediateLevelSubdivision> formalIntermediateLevelSubdivisionInserterFactory,
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
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory)
    {
        _formalIntermediateLevelSubdivisionInserterFactory = formalIntermediateLevelSubdivisionInserterFactory;
        _intermediateLevelSubdivisionInserterFactory = intermediateLevelSubdivisionInserterFactory;
        _isoCodedFirstLevelSubdivisionInserterFactory = isoCodedFirstLevelSubdivisionInserterFactory;
        _firstLevelSubdivisionInserterFactory = firstLevelSubdivisionInserterFactory;
        _isoCodedSubdivisionInserterFactory = isoCodedSubdivisionInserterFactory;
        _subdivisionInserterFactory = subdivisionInserterFactory;
        _politicalEntityInserterFactory = politicalEntityInserterFactory;
        _geographicalEntityInserterFactory = geographicalEntityInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _termReaderFactory = termReaderFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<FormalIntermediateLevelSubdivision> subdivisions, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var geographicalEntityWriter = await _geographicalEntityInserterFactory.CreateAsync(connection);
        await using var politicalEntityWriter = await _politicalEntityInserterFactory.CreateAsync(connection);
        await using var subdivisionWriter = await _subdivisionInserterFactory.CreateAsync(connection);
        await using var isoCodedSubdivisionWriter = await _isoCodedSubdivisionInserterFactory.CreateAsync(connection);
        await using var firstLevelSubdivisionWriter = await _firstLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var isoCodedFirstLevelSubdivisionWriter = await _isoCodedFirstLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var intermediateLevelSubdivisionWriter = await _intermediateLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var formalIntermediateLevelSubdivisionWriter = await _formalIntermediateLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

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
