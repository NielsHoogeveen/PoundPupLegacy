namespace PoundPupLegacy.CreateModel.Creators;

public class CongressionalTermPoliticalPartyAffiliationCreator : IEntityCreator<CongressionalTermPoliticalPartyAffiliation>
{
    public static async Task CreateAsync(IAsyncEnumerable<CongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var congressionalTermPoliticalPartyAffiliationWriter = await CongressionalTermPoliticalPartyAffiliationInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var congressionalTermPoliticalPartyAffiliation in congressionalTermPoliticalPartyAffiliations) {
            await nodeWriter.WriteAsync(congressionalTermPoliticalPartyAffiliation);
            await searchableWriter.WriteAsync(congressionalTermPoliticalPartyAffiliation);
            await documentableWriter.WriteAsync(congressionalTermPoliticalPartyAffiliation);
            await congressionalTermPoliticalPartyAffiliationWriter.WriteAsync(congressionalTermPoliticalPartyAffiliation);
            foreach (var tenantNode in congressionalTermPoliticalPartyAffiliation.TenantNodes) {
                tenantNode.NodeId = congressionalTermPoliticalPartyAffiliation.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
