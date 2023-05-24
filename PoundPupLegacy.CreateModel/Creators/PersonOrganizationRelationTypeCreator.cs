namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonOrganizationRelationTypeCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePersonOrganizationRelationType> personOrganizationRelationTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
) : INameableCreatorFactory<EventuallyIdentifiablePersonOrganizationRelationType>
{
    public async Task<NameableCreator<EventuallyIdentifiablePersonOrganizationRelationType>> CreateAsync(IDbConnection connection) =>
        new (
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
