namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HouseTermCreator : IEntityCreator<HouseTerm>
{
    public async Task CreateAsync(IAsyncEnumerable<HouseTerm> houseTerms, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var congressionalTermWriter = await CongressionalTermInserter.CreateAsync(connection);
        await using var houseTermWriter = await HouseTermInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var houseTerm in houseTerms) {
            await nodeWriter.InsertAsync(houseTerm);
            await searchableWriter.InsertAsync(houseTerm);
            await documentableWriter.InsertAsync(houseTerm);
            await congressionalTermWriter.InsertAsync(houseTerm);
            await houseTermWriter.InsertAsync(houseTerm);
            foreach (var partyAffiliation in houseTerm.PartyAffiliations) {
                partyAffiliation.CongressionalTermId = houseTerm.Id;
            }
            await new CongressionalTermPoliticalPartyAffiliationCreator().CreateAsync(houseTerm.PartyAffiliations.ToAsyncEnumerable(), connection);
            foreach (var tenantNode in houseTerm.TenantNodes) {
                tenantNode.NodeId = houseTerm.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
