namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterPersonalRelationTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<InterPersonalRelationType.InterPersonalRelationTypeToCreate> interPersonalRelationTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<InterPersonalRelationType.InterPersonalRelationTypeToCreate>
{
    public async Task<IEntityCreator<InterPersonalRelationType.InterPersonalRelationTypeToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<InterPersonalRelationType.InterPersonalRelationTypeToCreate>(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await interPersonalRelationTypeInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
