namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CountryAndIntermediateLevelSubdivisionCreator : EntityCreator<CountryAndIntermediateLevelSubdivision>
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
    private readonly IDatabaseInserterFactory<ISOCodedFirstLevelSubdivision> _isoCodedFirstLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<CountryAndFirstLevelSubdivision> _countryAndFirstLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<CountryAndIntermediateLevelSubdivision> _countryAndIntermediateLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<IntermediateLevelSubdivision> _intermediateLevelSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderByNameInserterFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderByOwnerAndNameFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public CountryAndIntermediateLevelSubdivisionCreator(
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
        IDatabaseInserterFactory<ISOCodedFirstLevelSubdivision> isoCodedFirstLevelSubdivisionInserterFactory, 
        IDatabaseInserterFactory<CountryAndFirstLevelSubdivision> countryAndFirstLevelSubdivisionInserterFactory, 
        IDatabaseInserterFactory<CountryAndIntermediateLevelSubdivision> countryAndIntermediateLevelSubdivisionInserterFactory, 
        IDatabaseInserterFactory<IntermediateLevelSubdivision> intermediateLevelSubdivisionInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderByNameInserterFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyFactory,
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
        _isoCodedFirstLevelSubdivisionInserterFactory = isoCodedFirstLevelSubdivisionInserterFactory;
        _countryAndFirstLevelSubdivisionInserterFactory = countryAndFirstLevelSubdivisionInserterFactory;
        _countryAndIntermediateLevelSubdivisionInserterFactory = countryAndIntermediateLevelSubdivisionInserterFactory;
        _intermediateLevelSubdivisionInserterFactory = intermediateLevelSubdivisionInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderByNameInserterFactory = termReaderByNameInserterFactory;
        _termHierarchyFactory = termHierarchyFactory;
        _vocabularyIdReaderByOwnerAndNameFactory = vocabularyIdReaderByOwnerAndNameFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<CountryAndIntermediateLevelSubdivision> countries, IDbConnection connection)
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
        await using var isoCodedFirstLevelSubdivisionWriter = await _isoCodedFirstLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var countryAndFirstLevelSubdivisionWriter = await _countryAndFirstLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var intermediateLevelSubdivisionWriter = await _intermediateLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var countryAndIntermediateLevelSubdivisionWriter = await _countryAndIntermediateLevelSubdivisionInserterFactory.CreateAsync(connection);
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
            await intermediateLevelSubdivisionWriter.InsertAsync(country);
            await countryAndIntermediateLevelSubdivisionWriter.InsertAsync(country);
            await WriteTerms(country, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in country.TenantNodes) {
                tenantNode.NodeId = country.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
