using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class BoundCountryCreator : IEntityCreator<BoundCountry>
{
    public static async Task CreateAsync(IAsyncEnumerable<BoundCountry> countries, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var geographicalEntityWriter = await GeographicalEnityWriter.CreateAsync(connection);
        await using var politicalEntityWriter = await PoliticalEntityWriter.CreateAsync(connection);
        await using var countryWriter = await CountryWriter.CreateAsync(connection);
        await using var subdivisionWriter = await SubdivisionWriter.CreateAsync(connection);
        await using var isoCodedSubdivisionWriter = await ISOCodedSubdivisionWriter.CreateAsync(connection);
        await using var boundCountryWriter = await BoundCountryWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);


        await foreach (var country in countries)
        {
            await nodeWriter.WriteAsync(country);
            await documentableWriter.WriteAsync(country);
            await nameableWriter.WriteAsync(country);
            await geographicalEntityWriter.WriteAsync(country);
            await politicalEntityWriter.WriteAsync(country);
            await countryWriter.WriteAsync(country);
            await subdivisionWriter.WriteAsync(country);
            await isoCodedSubdivisionWriter.WriteAsync(country);
            await boundCountryWriter.WriteAsync(country);
            await EntityCreator.WriteTerms(country, termWriter, termReader, termHierarchyWriter);
        }
    }
}
