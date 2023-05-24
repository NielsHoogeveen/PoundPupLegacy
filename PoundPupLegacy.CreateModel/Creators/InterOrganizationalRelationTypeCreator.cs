namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterOrganizationalRelationTypeCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableInterOrganizationalRelationType> interOrganizationalRelationTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
) : INameableCreatorFactory<EventuallyIdentifiableInterOrganizationalRelationType>
{
    public async Task<NameableCreator<EventuallyIdentifiableInterOrganizationalRelationType>> CreateAsync(IDbConnection connection) =>
        new(
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
