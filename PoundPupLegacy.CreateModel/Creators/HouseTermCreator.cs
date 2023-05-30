namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HouseTermCreatorFactory(

    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<CongressionalTermToCreate> congressionalTermInserterFactory,
    IDatabaseInserterFactory<HouseTerm.ToCreate> houseTermInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    IEntityCreatorFactory<CongressionalTermPoliticalPartyAffiliation.ToCreateForExistingTerm> congressionalTermPoliticalPartyAffiliationCreatorFactory
) : IEntityCreatorFactory<HouseTerm.ToCreate>
{
    public async Task<IEntityCreator<HouseTerm.ToCreate>> CreateAsync(IDbConnection connection) =>
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
    List<IDatabaseInserter<HouseTerm.ToCreate>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    IEntityCreator<CongressionalTermPoliticalPartyAffiliation.ToCreateForExistingTerm> congressionalTermPoliticalPartyAffiliationCreator
) : NodeCreator<HouseTerm.ToCreate>(inserters, nodeDetailsCreator)
{
    public override async Task ProcessAsync(HouseTerm.ToCreate element, int id)
    {
        await base.ProcessAsync(element, id);
        await congressionalTermPoliticalPartyAffiliationCreator
            .CreateAsync(element.CongressionalTermDetails.PartyAffiliations
            .Select(x => x.ResolveCongressionalTerm(id))
            .ToAsyncEnumerable());

    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await congressionalTermPoliticalPartyAffiliationCreator.DisposeAsync();
    }
}