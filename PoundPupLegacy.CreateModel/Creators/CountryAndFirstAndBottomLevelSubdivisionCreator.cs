namespace PoundPupLegacy.CreateModel.Creators;

public class CountryAndFirstAndBottomLevelSubdivisionCreator : IEntityCreator<CountryAndFirstAndBottomLevelSubdivision>
{
    public static async Task CreateAsync(IAsyncEnumerable<CountryAndFirstAndBottomLevelSubdivision> countries, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var geographicalEntityWriter = await GeographicalEnityInserter.CreateAsync(connection);
        await using var politicalEntityWriter = await PoliticalEntityInserter.CreateAsync(connection);
        await using var countryWriter = await CountryInserter.CreateAsync(connection);
        await using var topLevelCountryWriter = await TopLevelCountryInserter.CreateAsync(connection);
        await using var subdivisionWriter = await SubdivisionInserter.CreateAsync(connection);
        await using var isoCodedSubdivisionWriter = await ISOCodedSubdivisionInserter.CreateAsync(connection);
        await using var firstLevelSubdivisionWriter = await FirstLevelSubdivisionInserter.CreateAsync(connection);
        await using var isoCodedFirstLevelSubdivisionWriter = await ISOCodedFirstLevelSubdivisionInserter.CreateAsync(connection);
        await using var countryAndFirstLevelSubdivisionWriter = await CountryAndFirstLevelSubdivisionInserter.CreateAsync(connection);
        await using var bottomLevelSubdivisionWriter = await BottomLevelSubdivisionInserter.CreateAsync(connection);
        await using var countryAndFirstAndBottomLevelSubdivisionWriter = await CountryAndFirstAndBottomLevelSubdivisionInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);


        await foreach (var country in countries) {
            await nodeWriter.WriteAsync(country);
            await searchableWriter.WriteAsync(country);
            await documentableWriter.WriteAsync(country);
            await nameableWriter.WriteAsync(country);
            await geographicalEntityWriter.WriteAsync(country);
            await politicalEntityWriter.WriteAsync(country);
            await countryWriter.WriteAsync(country);
            await topLevelCountryWriter.WriteAsync(country);
            await subdivisionWriter.WriteAsync(country);
            await isoCodedSubdivisionWriter.WriteAsync(country);
            await firstLevelSubdivisionWriter.WriteAsync(country);
            await isoCodedFirstLevelSubdivisionWriter.WriteAsync(country);
            await countryAndFirstLevelSubdivisionWriter.WriteAsync(country);
            await bottomLevelSubdivisionWriter.WriteAsync(country);
            await countryAndFirstAndBottomLevelSubdivisionWriter.WriteAsync(country);
            await EntityCreator.WriteTerms(country, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in country.TenantNodes) {
                tenantNode.NodeId = country.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
