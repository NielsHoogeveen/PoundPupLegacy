namespace PoundPupLegacy.Db;

public class VocabularyCreator : IEntityCreator<Vocabulary>
{
    public static void Create(IEnumerable<Vocabulary> vocabularies, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var vocabularyWriter = VocabularyWriter.Create(connection);

        foreach (var vocabulary in vocabularies)
        {
            nodeWriter.Write(vocabulary);
            vocabularyWriter.Write(vocabulary);
        }
    }
}
