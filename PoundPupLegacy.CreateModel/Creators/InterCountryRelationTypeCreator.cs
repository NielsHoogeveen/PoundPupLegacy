namespace PoundPupLegacy.CreateModel.Creators;

public class InterCountryRelationTypeCreator : IEntityCreator<InterCountryRelationType>
{
    public static async Task CreateAsync(IAsyncEnumerable<InterCountryRelationType> interCountryRelationTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var interCountryRelationTypeWriter = await InterCountryRelationTypeInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var interCountryRelationType in interCountryRelationTypes) {
            await nodeWriter.InsertAsync(interCountryRelationType);
            await searchableWriter.InsertAsync(interCountryRelationType);
            await nameableWriter.InsertAsync(interCountryRelationType);
            await interCountryRelationTypeWriter.InsertAsync(interCountryRelationType);
            await EntityCreator.WriteTerms(interCountryRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in interCountryRelationType.TenantNodes) {
                tenantNode.NodeId = interCountryRelationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
