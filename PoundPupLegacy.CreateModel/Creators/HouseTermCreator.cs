namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HouseTermCreator(
    IDatabaseInserterFactory<NewHouseTerm> houseTermInserterFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<CongressionalTerm> congressionalTermInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
    IEntityCreator<NewCongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliationCreator
) : EntityCreator<NewHouseTerm>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewHouseTerm> houseTerms, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var congressionalTermWriter = await congressionalTermInserterFactory.CreateAsync(connection);
        await using var houseTermWriter = await houseTermInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var houseTerm in houseTerms) {
            await nodeWriter.InsertAsync(houseTerm);
            await searchableWriter.InsertAsync(houseTerm);
            await documentableWriter.InsertAsync(houseTerm);
            await congressionalTermWriter.InsertAsync(houseTerm);
            await houseTermWriter.InsertAsync(houseTerm);
            foreach (var partyAffiliation in houseTerm.PartyAffiliations) {
                partyAffiliation.CongressionalTermId = houseTerm.Id;
            }
            await congressionalTermPoliticalPartyAffiliationCreator.CreateAsync(houseTerm.PartyAffiliations.ToAsyncEnumerable(), connection);
            foreach (var tenantNode in houseTerm.TenantNodes) {
                tenantNode.NodeId = houseTerm.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
