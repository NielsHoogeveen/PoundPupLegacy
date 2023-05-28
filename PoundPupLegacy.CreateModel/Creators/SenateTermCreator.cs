namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenateTermCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<CongressionalTermToCreate> congressionalTermInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSenateTerm> senateTermInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    IEntityCreatorFactory<EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliationCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableSenateTerm>
{
    public async Task<IEntityCreator<EventuallyIdentifiableSenateTerm>> CreateAsync(IDbConnection connection) =>
        new SenateTermCreator(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await congressionalTermInserterFactory.CreateAsync(connection),
                await senateTermInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await congressionalTermPoliticalPartyAffiliationCreatorFactory.CreateAsync(connection)
        );
}
internal class SenateTermCreator(
    List<IDatabaseInserter<EventuallyIdentifiableSenateTerm>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    IEntityCreator<EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliationCreator
) : NodeCreator<EventuallyIdentifiableSenateTerm>(inserters, nodeDetailsCreator) 
{
    public override async Task ProcessAsync(EventuallyIdentifiableSenateTerm element, int id)
    {
        await base.ProcessAsync(element, id);
        foreach (var partyAffiliation in element.PartyAffiliations) {
            partyAffiliation.CongressionalTermId = id;
        }
        await congressionalTermPoliticalPartyAffiliationCreator.CreateAsync(element.PartyAffiliations.ToAsyncEnumerable());
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await congressionalTermPoliticalPartyAffiliationCreator.DisposeAsync();
    }
}