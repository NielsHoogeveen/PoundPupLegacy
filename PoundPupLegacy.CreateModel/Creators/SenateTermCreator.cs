namespace PoundPupLegacy.CreateModel.Creators;

public class SenateTermCreator : IEntityCreator<SenateTerm>
{
    public static async Task CreateAsync(IAsyncEnumerable<SenateTerm> senateTerms, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var congressionalTermWriter = await CongressionalTermInserter.CreateAsync(connection);
        await using var senateTermWriter = await SenateTermInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var senateTerm in senateTerms) {
            await nodeWriter.InsertAsync(senateTerm);
            await searchableWriter.InsertAsync(senateTerm);
            await documentableWriter.InsertAsync(senateTerm);
            await congressionalTermWriter.InsertAsync(senateTerm);
            await senateTermWriter.InsertAsync(senateTerm);
            foreach (var partyAffiliation in senateTerm.PartyAffiliations) {
                partyAffiliation.CongressionalTermId = senateTerm.Id;
            }
            await CongressionalTermPoliticalPartyAffiliationCreator.CreateAsync(senateTerm.PartyAffiliations.ToAsyncEnumerable(), connection);

            foreach (var tenantNode in senateTerm.TenantNodes) {
                tenantNode.NodeId = senateTerm.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
