namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DisruptedPlacementCaseCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableLocatable> locatableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableCase> caseInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDisruptedPlacementCase> disruptedPlacementCaseInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
) : INameableCreatorFactory<EventuallyIdentifiableDisruptedPlacementCase>
{
    public async Task<NameableCreator<EventuallyIdentifiableDisruptedPlacementCase>> CreateAsync(IDbConnection connection) =>
    new(
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
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
