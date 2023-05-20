namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterCountryRelationTypeCreator(
    IDatabaseInserterFactory<InterCountryRelationType> interCountryRelationTypeInserterFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<InterCountryRelationType>
{
    public override async Task CreateAsync(IAsyncEnumerable<InterCountryRelationType> interCountryRelationTypes, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var interCountryRelationTypeWriter = await interCountryRelationTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var interCountryRelationType in interCountryRelationTypes) {
            await nodeWriter.InsertAsync(interCountryRelationType);
            await searchableWriter.InsertAsync(interCountryRelationType);
            await nameableWriter.InsertAsync(interCountryRelationType);
            await interCountryRelationTypeWriter.InsertAsync(interCountryRelationType);
            await WriteTerms(interCountryRelationType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in interCountryRelationType.TenantNodes) {
                tenantNode.NodeId = interCountryRelationType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
