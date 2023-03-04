namespace PoundPupLegacy.Db;

public class PartyPoliticalEntityRelationCreator : IEntityCreator<PartyPoliticalEntityRelation>
{
    public static async Task CreateAsync(IAsyncEnumerable<PartyPoliticalEntityRelation> partyPoliticalEntityRelations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var partyPoliticalEntityRelationWriter = await PartyPoliticalEntityRelationWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

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
