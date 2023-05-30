namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterPersonalRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<InterPersonalRelation.ToCreateForExistingParticipants> interPersonalRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<InterPersonalRelation.ToCreateForExistingParticipants>
{
    public async Task<IEntityCreator<InterPersonalRelation.ToCreateForExistingParticipants>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<InterPersonalRelation.ToCreateForExistingParticipants>(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interPersonalRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
