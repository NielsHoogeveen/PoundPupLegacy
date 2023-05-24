namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HouseTermCreatorFactory(

    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableCongressionalTerm> congressionalTermInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableHouseTerm> houseTermInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    INodeCreatorFactory<EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliationCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableHouseTerm, HouseTermCreator>
{
    public async Task<HouseTermCreator> CreateAsync(IDbConnection connection) =>
        new(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await congressionalTermInserterFactory.CreateAsync(connection),
                await houseTermInserterFactory.CreateAsync(connection)

            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await congressionalTermPoliticalPartyAffiliationCreatorFactory.CreateAsync(connection)
        );
}

public class HouseTermCreator(
    List<IDatabaseInserter<EventuallyIdentifiableHouseTerm>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    IEntityCreator<EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliationCreator
) : NodeCreator<EventuallyIdentifiableHouseTerm>(inserters, nodeDetailsCreator)
{
    public override async Task ProcessAsync(EventuallyIdentifiableHouseTerm element)
    {
        await base.ProcessAsync(element);
        foreach (var partyAffiliation in element.PartyAffiliations) {
            partyAffiliation.CongressionalTermId = element.Id;
        }
        await congressionalTermPoliticalPartyAffiliationCreator.CreateAsync(element.PartyAffiliations.ToAsyncEnumerable());

    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await congressionalTermPoliticalPartyAffiliationCreator.DisposeAsync();
    }
}