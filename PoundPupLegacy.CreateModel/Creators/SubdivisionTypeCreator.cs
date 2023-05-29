namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SubdivisionTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<SubdivisionType.SubdivisionTypeToCreate> subdivisionTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<SubdivisionType.SubdivisionTypeToCreate>
{
    public async Task<IEntityCreator<SubdivisionType.SubdivisionTypeToCreate>> CreateAsync(IDbConnection connection) => 
        new NameableCreator<SubdivisionType.SubdivisionTypeToCreate>(
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
