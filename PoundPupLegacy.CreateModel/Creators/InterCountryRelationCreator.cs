namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterCountryRelationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableInterCountryRelation> interCountryRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableInterCountryRelation>
{
    public async Task<IEntityCreator<EventuallyIdentifiableInterCountryRelation>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<EventuallyIdentifiableInterCountryRelation>(
            new () 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interCountryRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
