namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenateTermCreator(
    IDatabaseInserterFactory<NewSenateTerm> senateTermInserterFactory,
    IDatabaseInserterFactory<CongressionalTerm> congressionalTermInserterFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
    IEntityCreator<NewCongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliationCreator
) : EntityCreator<NewSenateTerm>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewSenateTerm> senateTerms, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var congressionalTermWriter = await congressionalTermInserterFactory.CreateAsync(connection);
        await using var senateTermWriter = await senateTermInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var senateTerm in senateTerms) {
            await nodeWriter.InsertAsync(senateTerm);
            await searchableWriter.InsertAsync(senateTerm);
            await documentableWriter.InsertAsync(senateTerm);
            await congressionalTermWriter.InsertAsync(senateTerm);
            await senateTermWriter.InsertAsync(senateTerm);
            foreach (var partyAffiliation in senateTerm.PartyAffiliations) {
                partyAffiliation.CongressionalTermId = senateTerm.Id;
            }
            await congressionalTermPoliticalPartyAffiliationCreator.CreateAsync(senateTerm.PartyAffiliations.ToAsyncEnumerable(), connection);

            foreach (var tenantNode in senateTerm.TenantNodes) {
                tenantNode.NodeId = senateTerm.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
