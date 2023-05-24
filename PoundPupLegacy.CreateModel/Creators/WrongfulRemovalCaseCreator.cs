namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class WrongfulRemovalCaseCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableLocatable> locatableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableCase> caseInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableWrongfulRemovalCase> wrongfulRemovalCaseInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
) : INameableCreatorFactory<EventuallyIdentifiableWrongfulRemovalCase>
{
    public async Task<NameableCreator<EventuallyIdentifiableWrongfulRemovalCase>> CreateAsync(IDbConnection connection) =>
    new(
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
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
