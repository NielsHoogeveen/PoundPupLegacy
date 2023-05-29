namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PartyPoliticalEntityRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<PartyPoliticalEntityRelation.PartyPoliticalEntityRelationToCreateForExistingParty> partyPoliticalEntityRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<PartyPoliticalEntityRelation.PartyPoliticalEntityRelationToCreateForExistingParty>
{
    public async Task<IEntityCreator<PartyPoliticalEntityRelation.PartyPoliticalEntityRelationToCreateForExistingParty>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<PartyPoliticalEntityRelation.PartyPoliticalEntityRelationToCreateForExistingParty>(
            new (){
                await nodeInserterFactory.CreateAsync(connection),
                await partyPoliticalEntityRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );

}
