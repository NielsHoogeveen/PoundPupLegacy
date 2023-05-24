namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PartyPoliticalEntityRelationCreatorFactory(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePartyPoliticalEntityRelation> partyPoliticalEntityRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : INodeCreatorFactory<EventuallyIdentifiablePartyPoliticalEntityRelation>
{
    public async Task<NodeCreator<EventuallyIdentifiablePartyPoliticalEntityRelation>> CreateAsync(IDbConnection connection) =>
        new (
            new (){
                await nodeInserterFactory.CreateAsync(connection),
                await partyPoliticalEntityRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );

}
