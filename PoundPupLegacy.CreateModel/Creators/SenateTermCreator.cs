namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class SenateTermCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<CongressionalTermToCreate> congressionalTermInserterFactory,
    IDatabaseInserterFactory<SenateTerm.ToCreateForExistingSenator> senateTermInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    IEntityCreatorFactory<CongressionalTermPoliticalPartyAffiliation.ToCreateForExistingTerm> congressionalTermPoliticalPartyAffiliationCreatorFactory
) : IEntityCreatorFactory<SenateTerm.ToCreateForExistingSenator>
{
    public async Task<IEntityCreator<SenateTerm.ToCreateForExistingSenator>> CreateAsync(IDbConnection connection) =>
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
    List<IDatabaseInserter<SenateTerm.ToCreateForExistingSenator>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    IEntityCreator<CongressionalTermPoliticalPartyAffiliation.ToCreateForExistingTerm> congressionalTermPoliticalPartyAffiliationCreator
) : NodeCreator<SenateTerm.ToCreateForExistingSenator>(inserters, nodeDetailsCreator)
{
    public override async Task ProcessAsync(SenateTerm.ToCreateForExistingSenator element, int id)
    {
        await base.ProcessAsync(element, id);
        await congressionalTermPoliticalPartyAffiliationCreator
            .CreateAsync(element.CongressionalTermDetails.PartyAffiliations
            .Select(x => x.ResolveCongressionalTerm(id)).ToAsyncEnumerable());
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await congressionalTermPoliticalPartyAffiliationCreator.DisposeAsync();
    }
}