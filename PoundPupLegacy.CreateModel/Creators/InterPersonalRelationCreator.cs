namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterPersonalRelationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableInterPersonalRelationForExistingParticipants> interPersonalRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableInterPersonalRelationForExistingParticipants>
{
    public async Task<IEntityCreator<EventuallyIdentifiableInterPersonalRelationForExistingParticipants>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<EventuallyIdentifiableInterPersonalRelationForExistingParticipants>(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interPersonalRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
