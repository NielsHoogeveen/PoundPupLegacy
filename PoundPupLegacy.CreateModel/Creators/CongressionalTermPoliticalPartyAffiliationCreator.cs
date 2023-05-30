namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CongressionalTermPoliticalPartyAffiliationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory, 
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory, 
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory, 
    IDatabaseInserterFactory<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreateForExistingTerm> congressionalTermPoliticalPartyAffiliationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreateForExistingTerm>
{
    public async Task<IEntityCreator<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreateForExistingTerm>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreateForExistingTerm>(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await congressionalTermPoliticalPartyAffiliationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
