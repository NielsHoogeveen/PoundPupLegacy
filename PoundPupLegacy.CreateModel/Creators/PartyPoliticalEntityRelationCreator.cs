namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PartyPoliticalEntityRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<PartyPoliticalEntityRelation.ToCreateForExistingParty> partyPoliticalEntityRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<PartyPoliticalEntityRelation.ToCreateForExistingParty>
{
    public async Task<IEntityCreator<PartyPoliticalEntityRelation.ToCreateForExistingParty>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<PartyPoliticalEntityRelation.ToCreateForExistingParty>(
            new (){
                await nodeInserterFactory.CreateAsync(connection),
                await partyPoliticalEntityRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );

}
