namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CongressionalTermPoliticalPartyAffiliationCreator : IEntityCreator<CongressionalTermPoliticalPartyAffiliation>
{
    public async Task CreateAsync(IAsyncEnumerable<CongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliations, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var congressionalTermPoliticalPartyAffiliationWriter = await CongressionalTermPoliticalPartyAffiliationInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var congressionalTermPoliticalPartyAffiliation in congressionalTermPoliticalPartyAffiliations) {
            await nodeWriter.InsertAsync(congressionalTermPoliticalPartyAffiliation);
            await searchableWriter.InsertAsync(congressionalTermPoliticalPartyAffiliation);
            await documentableWriter.InsertAsync(congressionalTermPoliticalPartyAffiliation);
            await congressionalTermPoliticalPartyAffiliationWriter.InsertAsync(congressionalTermPoliticalPartyAffiliation);
            foreach (var tenantNode in congressionalTermPoliticalPartyAffiliation.TenantNodes) {
                tenantNode.NodeId = congressionalTermPoliticalPartyAffiliation.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
