namespace PoundPupLegacy.Db;

public class VocabularyCreator : IEntityCreator<Vocabulary>
{
    public static async Task CreateAsync(IAsyncEnumerable<Vocabulary> vocabularies, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var vocabularyWriter = await VocabularyWriter.CreateAsync(connection);

        await foreach (var vocabulary in vocabularies)
        {
            await nodeWriter.WriteAsync(vocabulary);
            await vocabularyWriter.WriteAsync(vocabulary);
        }
    }
}
