namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TypeOfAbuseCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<TypeOfAbuse.TypeOfAbuseToCreate> typeOfAbuseInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<TypeOfAbuse.TypeOfAbuseToCreate>
{
    public async Task<IEntityCreator<TypeOfAbuse.TypeOfAbuseToCreate>> CreateAsync(IDbConnection connection) => 
        new NameableCreator<TypeOfAbuse.TypeOfAbuseToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await typeOfAbuseInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
