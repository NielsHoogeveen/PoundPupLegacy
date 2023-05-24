namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ActCreatorFactory(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableAct> actInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
 ): INameableCreatorFactory<EventuallyIdentifiableAct>
{
    public async Task<NameableCreator<EventuallyIdentifiableAct>> CreateAsync(IDbConnection connection) =>
        new (
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await actInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
