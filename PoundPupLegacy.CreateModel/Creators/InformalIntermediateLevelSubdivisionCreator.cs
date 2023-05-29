﻿namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InformalIntermediateLevelSubdivisionCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<GeographicalEntityToCreate> geographicalEntityInserterFactory,
    IDatabaseInserterFactory<SubdivisionToCreate> subdivisionInserterFactory,
    IDatabaseInserterFactory<FirstLevelSubdivisionToCreate> firstLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<IntermediateLevelSubdivisionToCreate> intermediateLevelSubdivisionInserterFactory,
    IDatabaseInserterFactory<InformalIntermediateLevelSubdivision.InformalIntermediateLevelSubdivisionToCreate> informalIntermediateLevelSubdivisionInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<InformalIntermediateLevelSubdivision.InformalIntermediateLevelSubdivisionToCreate>
{
    public async Task<IEntityCreator<InformalIntermediateLevelSubdivision.InformalIntermediateLevelSubdivisionToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<InformalIntermediateLevelSubdivision.InformalIntermediateLevelSubdivisionToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await geographicalEntityInserterFactory.CreateAsync(connection),
                await subdivisionInserterFactory.CreateAsync(connection),
                await firstLevelSubdivisionInserterFactory.CreateAsync(connection),
                await intermediateLevelSubdivisionInserterFactory.CreateAsync(connection),
                await informalIntermediateLevelSubdivisionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
