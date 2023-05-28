namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PartyPoliticalEntityRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty> partyPoliticalEntityRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty>
{
    public async Task<IEntityCreator<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty>(
            new (){
                await nodeInserterFactory.CreateAsync(connection),
                await partyPoliticalEntityRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );

}
