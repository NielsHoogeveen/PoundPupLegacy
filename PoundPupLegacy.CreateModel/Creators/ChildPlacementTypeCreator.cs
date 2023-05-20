namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ChildPlacementTypeCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<ChildPlacementType> childPlacementTypeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<ChildPlacementType>
{
    public override async Task CreateAsync(IAsyncEnumerable<ChildPlacementType> childPlacementTypes, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var childPlacementTypeWriter = await childPlacementTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var childPlacementType in childPlacementTypes) {
            await nodeWriter.InsertAsync(childPlacementType);
            await searchableWriter.InsertAsync(childPlacementType);
            await nameableWriter.InsertAsync(childPlacementType);
            await childPlacementTypeWriter.InsertAsync(childPlacementType);
            await WriteTerms(childPlacementType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in childPlacementType.TenantNodes) {
                tenantNode.NodeId = childPlacementType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
