namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class WrongfulRemovalCaseCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<LocatableToCreate> locatableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<CaseToCreate> caseInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableWrongfulRemovalCase> wrongfulRemovalCaseInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory,
    LocatableDetailsCreatorFactory locatableDetailsCreatorFactory,
    IEntityCreatorFactory<CaseNewCasePartiesToUpdate> caseCaseTypeCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableWrongfulRemovalCase>
{
    public async Task<IEntityCreator<EventuallyIdentifiableWrongfulRemovalCase>> CreateAsync(IDbConnection connection) =>
        new CaseCreator<EventuallyIdentifiableWrongfulRemovalCase>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await locatableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await caseInserterFactory.CreateAsync(connection),
                await wrongfulRemovalCaseInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection),
            await locatableDetailsCreatorFactory.CreateAsync(connection),
            await caseCaseTypeCreatorFactory.CreateAsync(connection)
        );
}
