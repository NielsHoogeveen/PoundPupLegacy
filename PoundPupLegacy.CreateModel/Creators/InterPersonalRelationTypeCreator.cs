namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterPersonalRelationTypeCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<NewInterPersonalRelationType> interPersonalRelationTypeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewInterPersonalRelationType>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewInterPersonalRelationType> interPersonalRelationTypes, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var interPersonalRelationTypeWriter = await interPersonalRelationTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var interPersonalRelationType in interPersonalRelationTypes) {
            await nodeWriter.InsertAsync(interPersonalRelationType);
            await searchableWriter.InsertAsync(interPersonalRelationType);
            await nameableWriter.InsertAsync(interPersonalRelationType);
            await interPersonalRelationTypeWriter.InsertAsync(interPersonalRelationType);
            await WriteTerms(interPersonalRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in interPersonalRelationType.TenantNodes) {
                tenantNode.NodeId = interPersonalRelationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
