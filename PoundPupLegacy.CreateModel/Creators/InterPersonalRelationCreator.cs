namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterPersonalRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<InterPersonalRelation.InterPersonalRelationToCreateForExistingParticipants> interPersonalRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<InterPersonalRelation.InterPersonalRelationToCreateForExistingParticipants>
{
    public async Task<IEntityCreator<InterPersonalRelation.InterPersonalRelationToCreateForExistingParticipants>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<InterPersonalRelation.InterPersonalRelationToCreateForExistingParticipants>(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interPersonalRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
