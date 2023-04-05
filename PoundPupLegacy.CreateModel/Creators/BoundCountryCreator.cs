namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BoundCountryCreator : EntityCreator<BoundCountry>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<GeographicalEntity> _geographicalEntityInserterFactory;
    private readonly IDatabaseInserterFactory<PoliticalEntity> _politicalEntityInserterFactory;
    private readonly IDatabaseInserterFactory<Country> _countryInserterFactory;
    private readonly IDatabaseInserterFactory<Subdivision> _subdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<ISOCodedSubdivision> _isoCodedSubdivisionInserterFactory;
    private readonly IDatabaseInserterFactory<BoundCountry> _boundCountryInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;


    public BoundCountryCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Documentable> documentableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<GeographicalEntity> geographicalEntityInserterFactory,
        IDatabaseInserterFactory<PoliticalEntity> politicalEntityInserterFactory,
        IDatabaseInserterFactory<Country> countryInserterFactory,
        IDatabaseInserterFactory<Subdivision> subdivisionInserterFactory,
        IDatabaseInserterFactory<ISOCodedSubdivision> isoCodedSubdivisionInserterFactory,
        IDatabaseInserterFactory<BoundCountry> boundCountryInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
        )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _geographicalEntityInserterFactory = geographicalEntityInserterFactory;
        _politicalEntityInserterFactory = politicalEntityInserterFactory;
        _countryInserterFactory = countryInserterFactory;
        _subdivisionInserterFactory = subdivisionInserterFactory;
        _boundCountryInserterFactory = boundCountryInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;


    }

    public override async Task CreateAsync(IAsyncEnumerable<BoundCountry> countries, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var geographicalEntityWriter = await _geographicalEntityInserterFactory.CreateAsync(connection);
        await using var politicalEntityWriter = await _politicalEntityInserterFactory.CreateAsync(connection);
        await using var countryWriter = await _countryInserterFactory.CreateAsync(connection);
        await using var subdivisionWriter = await _subdivisionInserterFactory.CreateAsync(connection);
        await using var isoCodedSubdivisionWriter = await _isoCodedSubdivisionInserterFactory.CreateAsync(connection);
        await using var boundCountryWriter = await _boundCountryInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var country in countries) {
            await nodeWriter.InsertAsync(country);
            await searchableWriter.InsertAsync(country);
            await documentableWriter.InsertAsync(country);
            await nameableWriter.InsertAsync(country);
            await geographicalEntityWriter.InsertAsync(country);
            await politicalEntityWriter.InsertAsync(country);
            await countryWriter.InsertAsync(country);
            await subdivisionWriter.InsertAsync(country);
            await isoCodedSubdivisionWriter.InsertAsync(country);
            await boundCountryWriter.InsertAsync(country);
            await WriteTerms(country, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in country.TenantNodes) {
                tenantNode.NodeId = country.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
