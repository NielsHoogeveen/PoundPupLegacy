namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterPersonalRelationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableInterPersonalRelation> interPersonalRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableInterPersonalRelation>
{
    public async Task<IEntityCreator<EventuallyIdentifiableInterPersonalRelation>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<EventuallyIdentifiableInterPersonalRelation>(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interPersonalRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
