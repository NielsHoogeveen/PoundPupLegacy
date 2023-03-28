using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class UnitedStatesPoliticalPartyAffliationCreator : IEntityCreator<UnitedStatesPoliticalPartyAffliation>
{
    public static async Task CreateAsync(IAsyncEnumerable<UnitedStatesPoliticalPartyAffliation> unitedStatesPoliticalPartyAffliations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var unitedStatesPoliticalPartyAffliationWriter = await UnitedStatesPoliticalPartyAffliationWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await (new TermReaderByNameFactory()).CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await (new VocabularyIdReaderByOwnerAndNameFactory()).CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var unitedStatesPoliticalPartyAffliation in unitedStatesPoliticalPartyAffliations) {
            await nodeWriter.WriteAsync(unitedStatesPoliticalPartyAffliation);
            await searchableWriter.WriteAsync(unitedStatesPoliticalPartyAffliation);
            await nameableWriter.WriteAsync(unitedStatesPoliticalPartyAffliation);
            await unitedStatesPoliticalPartyAffliationWriter.WriteAsync(unitedStatesPoliticalPartyAffliation);
            await EntityCreator.WriteTerms(unitedStatesPoliticalPartyAffliation, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in unitedStatesPoliticalPartyAffliation.TenantNodes) {
                tenantNode.NodeId = unitedStatesPoliticalPartyAffliation.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
