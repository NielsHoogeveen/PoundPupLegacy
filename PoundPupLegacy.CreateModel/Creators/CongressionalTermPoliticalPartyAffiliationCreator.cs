using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class CongressionalTermPoliticalPartyAffiliationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<CongressionalTermPoliticalPartyAffiliation.ToCreateForExistingTerm> congressionalTermPoliticalPartyAffiliationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<CongressionalTermPoliticalPartyAffiliation.ToCreateForExistingTerm>
{
    public async Task<IEntityCreator<CongressionalTermPoliticalPartyAffiliation.ToCreateForExistingTerm>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<CongressionalTermPoliticalPartyAffiliation.ToCreateForExistingTerm>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await congressionalTermPoliticalPartyAffiliationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
