namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HouseTermCreatorFactory(

    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<CongressionalTermToCreate> congressionalTermInserterFactory,
    IDatabaseInserterFactory<HouseTerm.HouseTermToCreate> houseTermInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    IEntityCreatorFactory<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreate> congressionalTermPoliticalPartyAffiliationCreatorFactory
) : IEntityCreatorFactory<HouseTerm.HouseTermToCreate>
{
    public async Task<IEntityCreator<HouseTerm.HouseTermToCreate>> CreateAsync(IDbConnection connection) =>
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
    List<IDatabaseInserter<HouseTerm.HouseTermToCreate>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    IEntityCreator<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreate> congressionalTermPoliticalPartyAffiliationCreator
) : NodeCreator<HouseTerm.HouseTermToCreate>(inserters, nodeDetailsCreator)
{
    public override async Task ProcessAsync(HouseTerm.HouseTermToCreate element, int id)
    {
        await base.ProcessAsync(element, id);
        foreach (var partyAffiliation in element.CongressionalTermDetails.PartyAffiliations) {
            partyAffiliation.CongressionalTermPoliticalPartyAffiliationDetails.CongressionalTermId = id;
        }
        await congressionalTermPoliticalPartyAffiliationCreator.CreateAsync(element.CongressionalTermDetails.PartyAffiliations.ToAsyncEnumerable());

    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await congressionalTermPoliticalPartyAffiliationCreator.DisposeAsync();
    }
}