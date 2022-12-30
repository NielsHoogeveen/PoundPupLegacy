using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class BoundCountryCreator : IEntityCreator<BoundCountry>
{
    public static void Create(IEnumerable<BoundCountry> countries, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var geographicalEntityWriter = GeographicalEnityWriter.Create(connection);
        using var politicalEntityWriter = PoliticalEntityWriter.Create(connection);
        using var countryWriter = CountryWriter.Create(connection);
        using var subdivisionWriter = SubdivisionWriter.Create(connection);
        using var isoCodedSubdivisionWriter = ISOCodedSubdivisionWriter.Create(connection);
        using var boundCountryWriter = BoundCountryWriter.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var country in countries)
        {
            nodeWriter.Write(country);
            documentableWriter.Write(country);
            termWriter.Write(country);
            geographicalEntityWriter.Write(country);
            politicalEntityWriter.Write(country);
            countryWriter.Write(country);
            subdivisionWriter.Write(country);
            isoCodedSubdivisionWriter.Write(country);
            boundCountryWriter.Write(country);
            termHierarchyWriter.Write(new TermHierarchy
            {
                ParentId = country.BindingCountryId,
                ChildId = country.Id
            });
        }
    }
}
