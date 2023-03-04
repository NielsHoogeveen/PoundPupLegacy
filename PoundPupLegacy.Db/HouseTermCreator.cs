namespace PoundPupLegacy.Db;

public class HouseTermCreator : IEntityCreator<HouseTerm>
{
    public static async Task CreateAsync(IAsyncEnumerable<HouseTerm> houseTerms, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var congressionalTermWriter = await CongressionalTermWriter.CreateAsync(connection);
        await using var houseTermWriter = await HouseTermWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

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
