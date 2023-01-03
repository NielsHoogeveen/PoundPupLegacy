using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class BasicCountryCreator : IEntityCreator<BasicCountry>
{
    public static void Create(IEnumerable<BasicCountry> countries, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var geographicalEntityWriter = GeographicalEnityWriter.Create(connection);
        using var politicalEntityWriter = PoliticalEntityWriter.Create(connection);
        using var countryWriter = CountryWriter.Create(connection);
        using var topLevelCountryWriter = TopLevelCountryWriter.Create(connection);
        using var basicCountryWriter = BasicCountryWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);


        foreach (var country in countries)
        {
            nodeWriter.Write(country);
            documentableWriter.Write(country);
            nameableWriter.Write(country);
            geographicalEntityWriter.Write(country);
            politicalEntityWriter.Write(country);
            countryWriter.Write(country);
            topLevelCountryWriter.Write(country);
            basicCountryWriter.Write(country);
            EntityCreator.WriteTerms(country, termWriter, termReader, termHierarchyWriter);
        }
    }
}
