﻿namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PartyPoliticalEntityRelationCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<PartyPoliticalEntityRelation> partyPoliticalEntityRelationInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<PartyPoliticalEntityRelation>
{
    public override async Task CreateAsync(IAsyncEnumerable<PartyPoliticalEntityRelation> partyPoliticalEntityRelations, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var partyPoliticalEntityRelationWriter = await partyPoliticalEntityRelationInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var partyPoliticalEntityRelation in partyPoliticalEntityRelations) {
            await nodeWriter.InsertAsync(partyPoliticalEntityRelation);
            await partyPoliticalEntityRelationWriter.InsertAsync(partyPoliticalEntityRelation);

            foreach (var tenantNode in partyPoliticalEntityRelation.TenantNodes) {
                tenantNode.NodeId = partyPoliticalEntityRelation.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
