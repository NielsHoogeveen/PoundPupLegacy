﻿namespace PoundPupLegacy.Db;

public class CountryAndFirstAndBottomLevelSubdivisionCreator : IEntityCreator<CountryAndFirstAndBottomLevelSubdivision>
{
    public static void Create(IEnumerable<CountryAndFirstAndBottomLevelSubdivision> countries, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var geographicalEntityWriter = GeographicalEnityWriter.Create(connection);
        using var politicalEntityWriter = PoliticalEntityWriter.Create(connection);
        using var countryWriter = CountryWriter.Create(connection);
        using var topLevelCountryWriter = TopLevelCountryWriter.Create(connection);
        using var subdivisionWriter = SubdivisionWriter.Create(connection);
        using var isoCodedSubdivisionWriter = ISOCodedSubdivisionWriter.Create(connection);
        using var firstLevelSubdivisionWriter = FirstLevelSubdivisionWriter.Create(connection);
        using var isoCodedFirstLevelSubdivisionWriter = ISOCodedFirstLevelSubdivisionWriter.Create(connection);
        using var countryAndFirstLevelSubdivisionWriter = CountryAndFirstLevelSubdivisionWriter.Create(connection);
        using var bottomLevelSubdivisionWriter = BottomLevelSubdivisionWriter.Create(connection);
        using var countryAndFirstAndBottomLevelSubdivisionWriter = CountryAndFirstAndBottomLevelSubdivisionWriter.Create(connection);
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
            subdivisionWriter.Write(country);
            isoCodedSubdivisionWriter.Write(country);
            firstLevelSubdivisionWriter.Write(country);
            isoCodedFirstLevelSubdivisionWriter.Write(country);
            countryAndFirstLevelSubdivisionWriter.Write(country);
            bottomLevelSubdivisionWriter.Write(country);
            countryAndFirstAndBottomLevelSubdivisionWriter.Write(country);
            termHierarchyWriter.Write(new TermHierarchy
            {
                ParentId = country.GlobalRegionId,
                ChildId = country.Id
            });
        }
    }
}