namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HouseTermCreatorFactory(

    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<CongressionalTermToCreate> congressionalTermInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableHouseTerm> houseTermInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    IEntityCreatorFactory<EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliationCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableHouseTerm>
{
    public async Task<IEntityCreator<EventuallyIdentifiableHouseTerm>> CreateAsync(IDbConnection connection) =>
        new HouseTermCreator(
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
    public override async Task ProcessAsync(EventuallyIdentifiableHouseTerm element, int id)
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