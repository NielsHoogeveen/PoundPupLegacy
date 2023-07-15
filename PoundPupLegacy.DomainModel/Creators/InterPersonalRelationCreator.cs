namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class InterPersonalRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<InterPersonalRelation.ToCreate.ForExistingParticipants> interPersonalRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<InterPersonalRelation.ToCreate.ForExistingParticipants>
{
    public async Task<IEntityCreator<InterPersonalRelation.ToCreate.ForExistingParticipants>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<InterPersonalRelation.ToCreate.ForExistingParticipants>(
            new()
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interPersonalRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
