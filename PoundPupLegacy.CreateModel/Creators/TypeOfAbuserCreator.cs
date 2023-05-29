namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TypeOfAbuserCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<TypeOfAbuser.TypeOfAbuserToCreate> typeOfAbuserInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<TypeOfAbuser.TypeOfAbuserToCreate>
{
    public async Task<IEntityCreator<TypeOfAbuser.TypeOfAbuserToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<TypeOfAbuser.TypeOfAbuserToCreate>(
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
