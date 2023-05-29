namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CongressionalTermPoliticalPartyAffiliationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory, 
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory, 
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory, 
    IDatabaseInserterFactory<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreate> congressionalTermPoliticalPartyAffiliationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreate>
{
    public async Task<IEntityCreator<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreate>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreate>(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await congressionalTermPoliticalPartyAffiliationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
