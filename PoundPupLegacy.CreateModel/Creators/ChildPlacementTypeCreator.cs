namespace PoundPupLegacy.CreateModel.Creators;

public class ChildPlacementTypeCreator : IEntityCreator<ChildPlacementType>
{
    public static async Task CreateAsync(IAsyncEnumerable<ChildPlacementType> childPlacementTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var childPlacementTypeWriter = await ChildPlacementTypeInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var childPlacementType in childPlacementTypes) {
            await nodeWriter.WriteAsync(childPlacementType);
            await searchableWriter.WriteAsync(childPlacementType);
            await nameableWriter.WriteAsync(childPlacementType);
            await childPlacementTypeWriter.WriteAsync(childPlacementType);
            await EntityCreator.WriteTerms(childPlacementType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in childPlacementType.TenantNodes) {
                tenantNode.NodeId = childPlacementType.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
