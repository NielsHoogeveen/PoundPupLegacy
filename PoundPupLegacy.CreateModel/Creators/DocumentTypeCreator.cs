namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DocumentTypeCreator(
    IDatabaseInserterFactory<NewDocumentType> documentTypeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory
) : EntityCreator<NewDocumentType>
{

    public override async Task CreateAsync(IAsyncEnumerable<NewDocumentType> documentTypes, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var documentTypeWriter = await documentTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var documentType in documentTypes) {
            await nodeWriter.InsertAsync(documentType);
            await searchableWriter.InsertAsync(documentType);
            await nameableWriter.InsertAsync(documentType);
            await documentTypeWriter.InsertAsync(documentType);
            await WriteTerms(documentType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in documentType.TenantNodes) {
                tenantNode.NodeId = documentType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
