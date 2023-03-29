namespace PoundPupLegacy.CreateModel.Creators;

public class PartyPoliticalEntityRelationCreator : IEntityCreator<PartyPoliticalEntityRelation>
{
    public static async Task CreateAsync(IAsyncEnumerable<PartyPoliticalEntityRelation> partyPoliticalEntityRelations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var partyPoliticalEntityRelationWriter = await PartyPoliticalEntityRelationInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var partyPoliticalEntityRelation in partyPoliticalEntityRelations) {
            await nodeWriter.WriteAsync(partyPoliticalEntityRelation);
            await partyPoliticalEntityRelationWriter.WriteAsync(partyPoliticalEntityRelation);

            foreach (var tenantNode in partyPoliticalEntityRelation.TenantNodes) {
                tenantNode.NodeId = partyPoliticalEntityRelation.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
