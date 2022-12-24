using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class CountryAndFirstAndSecondLevelSubdivisionCreator : IEntityCreator<CountryAndFirstAndSecondLevelSubdivision>
{
    public static void Create(IEnumerable<CountryAndFirstAndSecondLevelSubdivision> countries, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var geographicalEntityWriter = GeographicalEnityWriter.Create(connection);
        using var politicalEntityWriter = PoliticalEnityWriter.Create(connection);
        using var countryWriter = CountryWriter.Create(connection);
        using var topLevelCountryWriter = TopLevelCountryWriter.Create(connection);
        using var subdivisionWriter = SubdivisionWriter.Create(connection);
        using var isoCodedSubdivisionWriter = ISOCodedSubdivisionWriter.Create(connection);
        using var firstLevelSubdivisionWriter = FirstLevelSubdivisionWriter.Create(connection);
        using var isoCodedFirstLevelSubdivisionWriter = ISOCodedFirstLevelSubdivisionWriter.Create(connection);
        using var countryAndFirstLevelSubdivisionWriter = CountryAndFirstLevelSubdivisionWriter.Create(connection);
        using var bottomLevelSubdivisionWriter = BottomLevelSubdivisionWriter.Create(connection);
        using var secondLevelSubdivisionWriter = SecondLevelSubdivisionWriter.Create(connection);
        using var firstAndSecondLevelSubdivisionWriter = FirstAndSecondLevelSubdivisionWriter.Create(connection);
        using var countryAndFirstAndSecondLevelSubdivisionWriter = CountryAndFirstAndSecondLevelSubdivisionWriter.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var country in countries)
        {
            nodeWriter.Write(country);
            geographicalEntityWriter.Write(country);
            politicalEntityWriter.Write(country);
            countryWriter.Write(country);
            topLevelCountryWriter.Write(country);
            subdivisionWriter.Write(country);
            isoCodedSubdivisionWriter.Write(country);
            firstLevelSubdivisionWriter.Write(country);
            isoCodedFirstLevelSubdivisionWriter.Write(country);
            countryAndFirstLevelSubdivisionWriter.Write(country);
            bottomLevelSubdivisionWriter.Write(country);
            secondLevelSubdivisionWriter.Write(country);
            firstAndSecondLevelSubdivisionWriter.Write(country);
            countryAndFirstAndSecondLevelSubdivisionWriter.Write(country);
            termHierarchyWriter.Write(new TermHierarchy
            {
                ParentId = country.GlobalRegionId,
                ChildId = country.Id
            });
        }
    }
}
