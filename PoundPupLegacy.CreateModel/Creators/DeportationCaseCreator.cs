namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DeportationCaseCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableLocatable> locatableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableCase> caseInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDeportationCase> deportationCaseInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
) : INameableCreatorFactory<EventuallyIdentifiableDeportationCase>
{
    public async Task<NameableCreator<EventuallyIdentifiableDeportationCase>> CreateAsync(IDbConnection connection) =>
    new(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await locatableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await caseInserterFactory.CreateAsync(connection),
                await deportationCaseInserterFactory.CreateAsync(connection)

            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
