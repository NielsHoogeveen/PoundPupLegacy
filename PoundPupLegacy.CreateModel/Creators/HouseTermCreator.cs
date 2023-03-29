namespace PoundPupLegacy.CreateModel.Creators;

public class HouseTermCreator : IEntityCreator<HouseTerm>
{
    public static async Task CreateAsync(IAsyncEnumerable<HouseTerm> houseTerms, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var congressionalTermWriter = await CongressionalTermInserter.CreateAsync(connection);
        await using var houseTermWriter = await HouseTermInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var houseTerm in houseTerms) {
            await nodeWriter.WriteAsync(houseTerm);
            await searchableWriter.WriteAsync(houseTerm);
            await documentableWriter.WriteAsync(houseTerm);
            await congressionalTermWriter.WriteAsync(houseTerm);
            await houseTermWriter.WriteAsync(houseTerm);
            foreach (var partyAffiliation in houseTerm.PartyAffiliations) {
                partyAffiliation.CongressionalTermId = houseTerm.Id;
            }
            await CongressionalTermPoliticalPartyAffiliationCreator.CreateAsync(houseTerm.PartyAffiliations.ToAsyncEnumerable(), connection);
            foreach (var tenantNode in houseTerm.TenantNodes) {
                tenantNode.NodeId = houseTerm.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
