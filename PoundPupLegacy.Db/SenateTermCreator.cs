using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class SenateTermCreator : IEntityCreator<SenateTerm>
{
    public static async Task CreateAsync(IAsyncEnumerable<SenateTerm> senateTerms, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var congressionalTermWriter = await CongressionalTermWriter.CreateAsync(connection);
        await using var senateTermWriter = await SenateTermWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var senateTerm in senateTerms)
        {
            await nodeWriter.WriteAsync(senateTerm);
            await searchableWriter.WriteAsync(senateTerm);
            await documentableWriter.WriteAsync(senateTerm);
            await congressionalTermWriter.WriteAsync(senateTerm);
            await senateTermWriter.WriteAsync(senateTerm);
            foreach (var partyAffiliation in senateTerm.PartyAffiliations) {
                partyAffiliation.CongressionalTermId = senateTerm.Id;
            }
            await CongressionalTermPoliticalPartyAffiliationCreator.CreateAsync(senateTerm.PartyAffiliations.ToAsyncEnumerable(), connection);

            foreach (var tenantNode in senateTerm.TenantNodes)
            {
                tenantNode.NodeId = senateTerm.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
