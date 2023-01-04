namespace PoundPupLegacy.Db;

public class DocumentCreator : IEntityCreator<Document>
{
    public static async Task CreateAsync(IAsyncEnumerable<Document> documents, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentWriter = await DocumentWriter.CreateAsync(connection);

        await foreach (var document in documents)
        {
            await nodeWriter.WriteAsync(document);
            await documentWriter.WriteAsync(document);
        }
    }
}
