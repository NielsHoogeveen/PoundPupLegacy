namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CountryAndFirstAndBottomLevelSubdivisionCreator : EntityCreator<CountryAndFirstAndBottomLevelSubdivision>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<GeographicalEntity> _geographicalEntityInserterFactory;
    private readonly IDatabaseInserterFactory<PoliticalEntity> _politicalEntityInserterFactory;
    private readonly IDatabaseInserterFactory<Country> _countryInserterFactory;
    private readonly IDatabaseInserterFactory<TopLevelCountry> _topLevelCountryInserterFactory;
    private readonly IDatabaseInserterFactory<Subdivision> _subdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<ISOCodedSubdivision> _isoCodedSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<FirstLevelSubdivision> _firstLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<ISOCodedFirstLevelSubdivision> _isofirstLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<BottomLevelSubdivision> _bottomLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<CountryAndFirstAndBottomLevelSubdivision> _countryAndFirstAndBottomLevelSubdivisionFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderByNameInserterFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderByOwnerAndNameFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public CountryAndFirstAndBottomLevelSubdivisionCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory, 
        IDatabaseInserterFactory<Searchable> searchableInserterFactory, 
        IDatabaseInserterFactory<Documentable> documentableInserterFactory, 
        IDatabaseInserterFactory<Nameable> nameableInserterFactory, 
        IDatabaseInserterFactory<GeographicalEntity> geographicalEntityInserterFactory, 
        IDatabaseInserterFactory<PoliticalEntity> politicalEntityInserterFactory, 
        IDatabaseInserterFactory<Country> countryInserterFactory, 
        IDatabaseInserterFactory<TopLevelCountry> topLevelCountryInserterFactory, 
        IDatabaseInserterFactory<Subdivision> subdivisionInserterFactory, 
        IDatabaseInserterFactory<ISOCodedSubdivision> isoCodedSubdivisionInserterFactory, 
        IDatabaseInserterFactory<FirstLevelSubdivision> firstLevelSubdivisionInserterFactory, 
        IDatabaseInserterFactory<ISOCodedFirstLevelSubdivision> isofirstLevelSubdivisionInserterFactory,
        IDatabaseInserterFactory<BottomLevelSubdivision> bottomLevelSubdivisionInserterFactory,
        IDatabaseInserterFactory<CountryAndFirstAndBottomLevelSubdivision> countryAndFirstAndBottomLevelSubdivisionFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderByNameInserterFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderByOwnerAndNameFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory)
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _geographicalEntityInserterFactory = geographicalEntityInserterFactory;
        _politicalEntityInserterFactory = politicalEntityInserterFactory;
        _countryInserterFactory = countryInserterFactory;
        _topLevelCountryInserterFactory = topLevelCountryInserterFactory;
        _subdivisionInserterFactory = subdivisionInserterFactory;
        _isoCodedSubdivisionInserterFactory = isoCodedSubdivisionInserterFactory;
        _firstLevelSubdivisionInserterFactory = firstLevelSubdivisionInserterFactory;
        _isofirstLevelSubdivisionInserterFactory = isofirstLevelSubdivisionInserterFactory;
        _bottomLevelSubdivisionInserterFactory = bottomLevelSubdivisionInserterFactory;
        _countryAndFirstAndBottomLevelSubdivisionFactory = countryAndFirstAndBottomLevelSubdivisionFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderByNameInserterFactory = termReaderByNameInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
        _termHierarchyFactory = termHierarchyFactory;
        _vocabularyIdReaderByOwnerAndNameFactory = vocabularyIdReaderByOwnerAndNameFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<CountryAndFirstAndBottomLevelSubdivision> countries, IDbConnection connection)
    {
        

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var geographicalEntityWriter = await _geographicalEntityInserterFactory.CreateAsync(connection);
        await using var politicalEntityWriter = await _politicalEntityInserterFactory.CreateAsync(connection);
        await using var countryWriter = await _countryInserterFactory.CreateAsync(connection);
        await using var topLevelCountryWriter = await _topLevelCountryInserterFactory.CreateAsync(connection);
        await using var subdivisionWriter = await _subdivisionInserterFactory.CreateAsync(connection);
        await using var isoCodedSubdivisionWriter = await _isoCodedSubdivisionInserterFactory.CreateAsync(connection);
        await using var firstLevelSubdivisionWriter = await _firstLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var isoCodedFirstLevelSubdivisionWriter = await _isofirstLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var countryAndFirstLevelSubdivisionWriter = await _countryAndFirstAndBottomLevelSubdivisionFactory.CreateAsync(connection);
        await using var bottomLevelSubdivisionWriter = await _bottomLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var countryAndFirstAndBottomLevelSubdivisionWriter = await _countryAndFirstAndBottomLevelSubdivisionFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderByNameInserterFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderByOwnerAndNameFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);


        await foreach (var country in countries) {
            await nodeWriter.InsertAsync(country);
            await searchableWriter.InsertAsync(country);
            await documentableWriter.InsertAsync(country);
            await nameableWriter.InsertAsync(country);
            await geographicalEntityWriter.InsertAsync(country);
            await politicalEntityWriter.InsertAsync(country);
            await countryWriter.InsertAsync(country);
            await topLevelCountryWriter.InsertAsync(country);
            await subdivisionWriter.InsertAsync(country);
            await isoCodedSubdivisionWriter.InsertAsync(country);
            await firstLevelSubdivisionWriter.InsertAsync(country);
            await isoCodedFirstLevelSubdivisionWriter.InsertAsync(country);
            await countryAndFirstLevelSubdivisionWriter.InsertAsync(country);
            await bottomLevelSubdivisionWriter.InsertAsync(country);
            await countryAndFirstAndBottomLevelSubdivisionWriter.InsertAsync(country);
            await WriteTerms(country, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in country.TenantNodes) {
                tenantNode.NodeId = country.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
