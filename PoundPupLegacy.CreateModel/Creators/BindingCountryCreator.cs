namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BindingCountryCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<GeographicalEntity> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<PoliticalEntity> politicalEntityInserterFactory,
    IDatabaseInserterFactory<Country> countryInserterFactory,
    IDatabaseInserterFactory<TopLevelCountry> topLevelCountryInserterFactory,
    IDatabaseInserterFactory<NewBindingCountry> bindingCountryInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
    IEntityCreator<NewVocabulary> vocabularyCreator
) : EntityCreator<NewBindingCountry>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewBindingCountry> countries, IDbConnection connection)
    {

        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var geographicalEntityWriter = await geographicalEntityInserterFactory.CreateAsync(connection);
        await using var politicalEntityWriter = await politicalEntityInserterFactory.CreateAsync(connection);
        await using var countryWriter = await countryInserterFactory.CreateAsync(connection);
        await using var topLevelCountryWriter = await topLevelCountryInserterFactory.CreateAsync(connection);
        await using var bindingCountryWriter = await bindingCountryInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
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
            await bindingCountryWriter.InsertAsync(country);
            await WriteTerms(country, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in country.TenantNodes) {
                tenantNode.NodeId = country.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
