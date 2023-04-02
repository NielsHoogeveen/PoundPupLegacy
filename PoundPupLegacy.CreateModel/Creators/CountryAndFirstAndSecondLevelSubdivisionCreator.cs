﻿namespace PoundPupLegacy.CreateModel.Creators;

public class CountryAndFirstAndSecondLevelSubdivisionCreator : IEntityCreator<CountryAndFirstAndSecondLevelSubdivision>
{
    public async Task CreateAsync(IAsyncEnumerable<CountryAndFirstAndSecondLevelSubdivision> countries, IDbConnection connection)
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
        await using var secondLevelSubdivisionWriter = await SecondLevelSubdivisionInserter.CreateAsync(connection);
        await using var firstAndSecondLevelSubdivisionWriter = await FirstAndSecondLevelSubdivisionInserter.CreateAsync(connection);
        await using var countryAndFirstAndSecondLevelSubdivisionWriter = await CountryAndFirstAndSecondLevelSubdivisionInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);


        await foreach (var country in countries) {
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
            await bottomLevelSubdivisionWriter.InsertAsync(country);
            await secondLevelSubdivisionWriter.InsertAsync(country);
            await firstAndSecondLevelSubdivisionWriter.InsertAsync(country);
            await countryAndFirstAndSecondLevelSubdivisionWriter.InsertAsync(country);
            await EntityCreator.WriteTerms(country, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in country.TenantNodes) {
                tenantNode.NodeId = country.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
