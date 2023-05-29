namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicNameableCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<BasicNameable.BasicNameableToCreate> basicNameableInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<BasicNameable.BasicNameableToCreate>
{
    public async Task<IEntityCreator<BasicNameable.BasicNameableToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<BasicNameable.BasicNameableToCreate>(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await basicNameableInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
