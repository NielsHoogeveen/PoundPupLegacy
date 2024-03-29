﻿namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class CountryAndIntermediateLevelSubdivisionCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<GeographicalEntityToCreate> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<PoliticalEntityToCreate> politicalEntityInserterFactory,
    IDatabaseInserterFactory<CountryToCreate> countryInserterFactory,
    IDatabaseInserterFactory<TopLevelCountryToCreate> topLevelCountryInserterFactory,
    IDatabaseInserterFactory<SubdivisionToCreate> subdivisionInserterFactory,
    IDatabaseInserterFactory<ISOCodedSubdivisionToCreate> isoCodedSubdivisionInserterFactory,
    IDatabaseInserterFactory<FirstLevelSubdivisionToCreate> firstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<ISOCodedFirstLevelSubdivisionToCreate> isoCodedFirstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<CountryAndFirstLevelSubdivisionToCreate> countryAndFirstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<CountryAndIntermediateLevelSubdivision.ToCreate> countryAndIntermediateLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<IntermediateLevelSubdivisionToCreate> intermediateLevelSubdivisionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<CountryAndIntermediateLevelSubdivision.ToCreate>
{
    public async Task<IEntityCreator<CountryAndIntermediateLevelSubdivision.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<CountryAndIntermediateLevelSubdivision.ToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await geographicalEntityInserterFactory.CreateAsync(connection),
                await politicalEntityInserterFactory.CreateAsync(connection),
                await countryInserterFactory.CreateAsync(connection),
                await topLevelCountryInserterFactory.CreateAsync(connection),
                await subdivisionInserterFactory.CreateAsync(connection),
                await isoCodedSubdivisionInserterFactory.CreateAsync(connection),
                await firstLevelSubdivisionInserterFactory.CreateAsync(connection),
                await isoCodedFirstLevelSubdivisionInserterFactory.CreateAsync(connection),
                await countryAndFirstLevelSubdivisionInserterFactory.CreateAsync(connection),
                await intermediateLevelSubdivisionInserterFactory.CreateAsync(connection),
                await countryAndIntermediateLevelSubdivisionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
