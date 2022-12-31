namespace PoundPupLegacy.Db;

public class BindingCountryCreator : IEntityCreator<BindingCountry>
{
    public static void Create(IEnumerable<BindingCountry> countries, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var geographicalEntityWriter = GeographicalEnityWriter.Create(connection);
        using var politicalEntityWriter = PoliticalEntityWriter.Create(connection);
        using var countryWriter = CountryWriter.Create(connection);
        using var topLevelCountryWriter = TopLevelCountryWriter.Create(connection);
        using var bindingCountryWriter = BindingCountryWriter.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);


        foreach (var country in countries)
        {
            nodeWriter.Write(country);
            documentableWriter.Write(country);
            termWriter.Write(country);
            geographicalEntityWriter.Write(country);
            politicalEntityWriter.Write(country);
            countryWriter.Write(country);
            topLevelCountryWriter.Write(country);
            bindingCountryWriter.Write(country);
            termHierarchyWriter.Write(new TermHierarchy
            {
                ParentId = country.GlobalRegionId,
                ChildId = country.Id
            });
        }
    }
}
