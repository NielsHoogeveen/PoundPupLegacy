namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonOrganizationRelationTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<PersonOrganizationRelationType.PersonOrganizationRelationTypeToCreate> personOrganizationRelationTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<PersonOrganizationRelationType.PersonOrganizationRelationTypeToCreate>
{
    public async Task<IEntityCreator<PersonOrganizationRelationType.PersonOrganizationRelationTypeToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<PersonOrganizationRelationType.PersonOrganizationRelationTypeToCreate>(
            new ()
            {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await personOrganizationRelationTypeInserterFactory.CreateAsync(connection)
            }, 
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
