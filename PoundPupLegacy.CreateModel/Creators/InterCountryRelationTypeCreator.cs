namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterCountryRelationTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<InterCountryRelationType.ToCreate> interCountryRelationTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<InterCountryRelationType.ToCreate>
{
    public async Task<IEntityCreator<InterCountryRelationType.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<InterCountryRelationType.ToCreate>(
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
