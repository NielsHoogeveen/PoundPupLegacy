namespace PoundPupLegacy.CreateModel.Creators;

public class DocumentTypeCreator : IEntityCreator<DocumentType>
{
    public static async Task CreateAsync(IAsyncEnumerable<DocumentType> documentTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var documentTypeWriter = await DocumentTypeInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var documentType in documentTypes) {
            await nodeWriter.WriteAsync(documentType);
            await searchableWriter.WriteAsync(documentType);
            await nameableWriter.WriteAsync(documentType);
            await documentTypeWriter.WriteAsync(documentType);
            await EntityCreator.WriteTerms(documentType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in documentType.TenantNodes) {
                tenantNode.NodeId = documentType.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
