namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CountryAndFirstAndBottomLevelSubdivisionCreator(
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
    IDatabaseInserterFactory<CountryAndFirstLevelSubdivision> countryAndFirstLevelSubdivisionFactory,
    IDatabaseInserterFactory<NewCountryAndFirstAndBottomLevelSubdivision> countryAndFirstAndBottomLevelSubdivisionFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderByNameInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderByOwnerAndNameFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
    IEntityCreator<NewVocabulary> vocabularyCreator
) : EntityCreator<NewCountryAndFirstAndBottomLevelSubdivision>
{

    public override async Task CreateAsync(IAsyncEnumerable<NewCountryAndFirstAndBottomLevelSubdivision> countries, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var geographicalEntityWriter = await geographicalEntityInserterFactory.CreateAsync(connection);
        await using var politicalEntityWriter = await politicalEntityInserterFactory.CreateAsync(connection);
        await using var countryWriter = await countryInserterFactory.CreateAsync(connection);
        await using var topLevelCountryWriter = await topLevelCountryInserterFactory.CreateAsync(connection);
        await using var subdivisionWriter = await subdivisionInserterFactory.CreateAsync(connection);
        await using var isoCodedSubdivisionWriter = await isoCodedSubdivisionInserterFactory.CreateAsync(connection);
        await using var firstLevelSubdivisionWriter = await firstLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var isoCodedFirstLevelSubdivisionWriter = await isofirstLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var countryAndFirstLevelSubdivisionWriter = await countryAndFirstLevelSubdivisionFactory.CreateAsync(connection);
        await using var bottomLevelSubdivisionWriter = await bottomLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var countryAndFirstAndBottomLevelSubdivisionWriter = await countryAndFirstAndBottomLevelSubdivisionFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderByNameInserterFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderByOwnerAndNameFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);


        await foreach (var country in countries) {
            var vocabulary = new NewVocabulary {
                Id = null,
                Name = $"Subdivision names of {country.Name}",
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = $"Subdivision names of {country.Name}",
                OwnerId = Constants.OWNER_GEOGRAPHY,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = null
                    }
                },
                NodeTypeId = 38,
                Description = $"Contains unique names for all subdivisions of {country.Name}"
            };
            await vocabularyCreator.CreateAsync(vocabulary, connection);
            country.VocabularyIdSubdivisions = vocabulary.Id;
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
