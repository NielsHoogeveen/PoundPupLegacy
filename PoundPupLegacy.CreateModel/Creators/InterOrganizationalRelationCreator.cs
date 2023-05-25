namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterOrganizationalRelationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants> interOrganizationalRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants>
{
    public async Task<IEntityCreator<EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants>(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interOrganizationalRelationInserterFactory.CreateAsync(connection)

            },
             await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
