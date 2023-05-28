﻿namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FathersRightsViolationCaseCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<LocatableToCreate> locatableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<CaseToCreate> caseInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableFathersRightsViolationCase> fathersRightsViolationCaseInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory,
    LocatableDetailsCreatorFactory locatableDetailsCreatorFactory,
    IEntityCreatorFactory<CaseNewCasePartiesToUpdate> caseCaseTypeCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableFathersRightsViolationCase>
{
    public async Task<IEntityCreator<EventuallyIdentifiableFathersRightsViolationCase>> CreateAsync(IDbConnection connection) =>
        new CaseCreator<EventuallyIdentifiableFathersRightsViolationCase>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await locatableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await caseInserterFactory.CreateAsync(connection),
                await fathersRightsViolationCaseInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection),
            await locatableDetailsCreatorFactory.CreateAsync(connection),
            await caseCaseTypeCreatorFactory.CreateAsync(connection)
        );
}
