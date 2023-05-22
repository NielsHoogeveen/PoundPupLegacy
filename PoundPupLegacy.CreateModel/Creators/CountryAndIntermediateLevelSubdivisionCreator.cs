namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CountryAndIntermediateLevelSubdivisionCreator(
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
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderByNameInserterFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderByOwnerAndNameFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
    IEntityCreator<NewVocabulary> vocabularyCreator
) : EntityCreator<NewCountryAndIntermediateLevelSubdivision>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewCountryAndIntermediateLevelSubdivision> countries, IDbConnection connection)
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
        await using var isoCodedFirstLevelSubdivisionWriter = await isoCodedFirstLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var countryAndFirstLevelSubdivisionWriter = await countryAndFirstLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var intermediateLevelSubdivisionWriter = await intermediateLevelSubdivisionInserterFactory.CreateAsync(connection);
        await using var countryAndIntermediateLevelSubdivisionWriter = await countryAndIntermediateLevelSubdivisionInserterFactory.CreateAsync(connection);
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
