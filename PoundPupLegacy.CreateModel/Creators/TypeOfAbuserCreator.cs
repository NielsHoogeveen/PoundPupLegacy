namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TypeOfAbuserCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<TypeOfAbuser.ToCreate> typeOfAbuserInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<TypeOfAbuser.ToCreate>
{
    public async Task<IEntityCreator<TypeOfAbuser.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<TypeOfAbuser.ToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await typeOfAbuserInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
