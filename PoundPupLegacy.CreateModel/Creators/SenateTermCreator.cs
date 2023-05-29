namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenateTermCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<CongressionalTermToCreate> congressionalTermInserterFactory,
    IDatabaseInserterFactory<SenateTerm.SenateTermToCreate> senateTermInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    IEntityCreatorFactory<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreate> congressionalTermPoliticalPartyAffiliationCreatorFactory
) : IEntityCreatorFactory<SenateTerm.SenateTermToCreate>
{
    public async Task<IEntityCreator<SenateTerm.SenateTermToCreate>> CreateAsync(IDbConnection connection) =>
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
    List<IDatabaseInserter<SenateTerm.SenateTermToCreate>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    IEntityCreator<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreate> congressionalTermPoliticalPartyAffiliationCreator
) : NodeCreator<SenateTerm.SenateTermToCreate>(inserters, nodeDetailsCreator) 
{
    public override async Task ProcessAsync(SenateTerm.SenateTermToCreate element, int id)
    {
        await base.ProcessAsync(element, id);
        foreach (var partyAffiliation in element.SenateTermDetails.PartyAffiliations) {
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