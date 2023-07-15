﻿namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class ChildTraffickingCaseCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<LocatableToCreate> locatableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<CaseToCreate> caseInserterFactory,
    IDatabaseInserterFactory<ChildTraffickingCase.ToCreate> childTraffickingCaseInserterFactory,
    LocatableDetailsCreatorFactory locatableDetailsCreatorFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory,
    IEntityCreatorFactory<CaseCaseParties.ToCreate.ForExistingCase> caseCaseTypeCreatorFactory
) : IEntityCreatorFactory<ChildTraffickingCase.ToCreate>
{
    public async Task<IEntityCreator<ChildTraffickingCase.ToCreate>> CreateAsync(IDbConnection connection) =>
        new CaseCreator<ChildTraffickingCase.ToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await locatableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await caseInserterFactory.CreateAsync(connection),
                await childTraffickingCaseInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection),
            await locatableDetailsCreatorFactory.CreateAsync(connection),
            await caseCaseTypeCreatorFactory.CreateAsync(connection)
        );
}
