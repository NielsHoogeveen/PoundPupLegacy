using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class CountryAndFirstAndBottomLevelSubdivisionCreator : IEntityCreator<CountryAndFirstAndBottomLevelSubdivision>
{
    public static async Task CreateAsync(IAsyncEnumerable<CountryAndFirstAndBottomLevelSubdivision> countries, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var geographicalEntityWriter = await GeographicalEnityWriter.CreateAsync(connection);
        await using var politicalEntityWriter = await PoliticalEntityWriter.CreateAsync(connection);
        await using var countryWriter = await CountryWriter.CreateAsync(connection);
        await using var topLevelCountryWriter = await TopLevelCountryWriter.CreateAsync(connection);
        await using var subdivisionWriter = await SubdivisionWriter.CreateAsync(connection);
        await using var isoCodedSubdivisionWriter = await ISOCodedSubdivisionWriter.CreateAsync(connection);
        await using var firstLevelSubdivisionWriter = await FirstLevelSubdivisionWriter.CreateAsync(connection);
        await using var isoCodedFirstLevelSubdivisionWriter = await ISOCodedFirstLevelSubdivisionWriter.CreateAsync(connection);
        await using var countryAndFirstLevelSubdivisionWriter = await CountryAndFirstLevelSubdivisionWriter.CreateAsync(connection);
        await using var bottomLevelSubdivisionWriter = await BottomLevelSubdivisionWriter.CreateAsync(connection);
        await using var countryAndFirstAndBottomLevelSubdivisionWriter = await CountryAndFirstAndBottomLevelSubdivisionWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);


        await foreach (var country in countries)
        {
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
            foreach (var tenantNode in country.TenantNodes)
            {
                tenantNode.NodeId = country.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
