namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PartyPoliticalEntityRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<PartyPoliticalEntityRelation.ToCreate.ForExistingParty> partyPoliticalEntityRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<PartyPoliticalEntityRelation.ToCreate.ForExistingParty>
{
    public async Task<IEntityCreator<PartyPoliticalEntityRelation.ToCreate.ForExistingParty>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<PartyPoliticalEntityRelation.ToCreate.ForExistingParty>(
            new (){
                await nodeInserterFactory.CreateAsync(connection),
                await partyPoliticalEntityRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );

}
