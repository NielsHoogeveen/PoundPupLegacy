using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class HouseTermCreatorFactory(

    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<CongressionalTermToCreate> congressionalTermInserterFactory,
    IDatabaseInserterFactory<HouseTerm.ToCreateForExistingRepresentative> houseTermInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    IEntityCreatorFactory<CongressionalTermPoliticalPartyAffiliation.ToCreateForExistingTerm> congressionalTermPoliticalPartyAffiliationCreatorFactory
) : IEntityCreatorFactory<HouseTerm.ToCreateForExistingRepresentative>
{
    public async Task<IEntityCreator<HouseTerm.ToCreateForExistingRepresentative>> CreateAsync(IDbConnection connection) =>
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
    List<IDatabaseInserter<HouseTerm.ToCreateForExistingRepresentative>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    IEntityCreator<CongressionalTermPoliticalPartyAffiliation.ToCreateForExistingTerm> congressionalTermPoliticalPartyAffiliationCreator
) : NodeCreator<HouseTerm.ToCreateForExistingRepresentative>(inserters, nodeDetailsCreator)
{
    public override async Task ProcessAsync(HouseTerm.ToCreateForExistingRepresentative element, int id)
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