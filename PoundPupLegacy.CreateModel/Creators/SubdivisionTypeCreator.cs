namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SubdivisionTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSubdivisionType> subdivisionTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableSubdivisionType>
{
    public async Task<IEntityCreator<EventuallyIdentifiableSubdivisionType>> CreateAsync(IDbConnection connection) => 
        new NameableCreator<EventuallyIdentifiableSubdivisionType>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await subdivisionTypeInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
