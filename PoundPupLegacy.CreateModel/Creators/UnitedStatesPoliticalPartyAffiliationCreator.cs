namespace PoundPupLegacy.CreateModel.Creators;

public class UnitedStatesPoliticalPartyAffliationCreator : IEntityCreator<UnitedStatesPoliticalPartyAffliation>
{
    public static async Task CreateAsync(IAsyncEnumerable<UnitedStatesPoliticalPartyAffliation> unitedStatesPoliticalPartyAffliations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var unitedStatesPoliticalPartyAffliationWriter = await UnitedStatesPoliticalPartyAffliationInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

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
