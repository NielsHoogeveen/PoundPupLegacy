namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterCountryRelationTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<InterCountryRelationType.InterCountryRelationTypeToCreate> interCountryRelationTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<InterCountryRelationType.InterCountryRelationTypeToCreate>
{
    public async Task<IEntityCreator<InterCountryRelationType.InterCountryRelationTypeToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<InterCountryRelationType.InterCountryRelationTypeToCreate>(
            new()
            {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await interCountryRelationTypeInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
