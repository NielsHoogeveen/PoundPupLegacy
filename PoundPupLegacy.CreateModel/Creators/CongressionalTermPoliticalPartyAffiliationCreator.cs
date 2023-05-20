namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CongressionalTermPoliticalPartyAffiliationCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory, 
    IDatabaseInserterFactory<Searchable> searchableInserterFactory, 
    IDatabaseInserterFactory<Documentable> documentableInserterFactory, 
    IDatabaseInserterFactory<CongressionalTermPoliticalPartyAffiliation> 
    congressionalTermPoliticalPartyAffiliationInserterFactory, 
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<CongressionalTermPoliticalPartyAffiliation>
{
    public override async Task CreateAsync(IAsyncEnumerable<CongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliations, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var congressionalTermPoliticalPartyAffiliationWriter = await congressionalTermPoliticalPartyAffiliationInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

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
