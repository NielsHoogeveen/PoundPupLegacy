namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterOrganizationalRelationTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<InterOrganizationalRelationType.InterOrganizationalRelationTypeToCreate> interOrganizationalRelationTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<InterOrganizationalRelationType.InterOrganizationalRelationTypeToCreate>
{
    public async Task<IEntityCreator<InterOrganizationalRelationType.InterOrganizationalRelationTypeToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<InterOrganizationalRelationType.InterOrganizationalRelationTypeToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await interOrganizationalRelationTypeInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
