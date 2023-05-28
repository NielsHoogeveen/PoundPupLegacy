﻿namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DisruptedPlacementCaseCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<LocatableToCreate> locatableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<CaseToCreate> caseInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDisruptedPlacementCase> disruptedPlacementCaseInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory,
    LocatableDetailsCreatorFactory locatableDetailsCreatorFactory,
    IEntityCreatorFactory<CaseNewCasePartiesToUpdate> caseCaseTypeCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableDisruptedPlacementCase>
{
    public async Task<IEntityCreator<EventuallyIdentifiableDisruptedPlacementCase>> CreateAsync(IDbConnection connection) =>
        new CaseCreator<EventuallyIdentifiableDisruptedPlacementCase>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await locatableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await caseInserterFactory.CreateAsync(connection),
                await disruptedPlacementCaseInserterFactory.CreateAsync(connection)

            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection),
            await locatableDetailsCreatorFactory.CreateAsync(connection),
            await caseCaseTypeCreatorFactory.CreateAsync(connection)
        );
}
