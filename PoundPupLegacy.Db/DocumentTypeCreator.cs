using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class DocumentTypeCreator : IEntityCreator<DocumentType>
{
    public static async Task CreateAsync(IAsyncEnumerable<DocumentType> documentTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var documentTypeWriter = await DocumentTypeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

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
