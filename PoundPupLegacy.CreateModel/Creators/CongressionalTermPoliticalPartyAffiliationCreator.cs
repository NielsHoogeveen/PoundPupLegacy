namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CongressionalTermPoliticalPartyAffiliationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory, 
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory, 
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory, 
    IDatabaseInserterFactory<EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : INodeCreatorFactory<EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation>
{
    public async Task<NodeCreator<EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation>> CreateAsync(IDbConnection connection) =>
        new (
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await congressionalTermPoliticalPartyAffiliationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
