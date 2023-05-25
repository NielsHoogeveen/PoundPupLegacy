namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CongressionalTermPoliticalPartyAffiliationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory, 
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory, 
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory, 
    IDatabaseInserterFactory<EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation>
{
    public async Task<IEntityCreator<EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation>(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await congressionalTermPoliticalPartyAffiliationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
