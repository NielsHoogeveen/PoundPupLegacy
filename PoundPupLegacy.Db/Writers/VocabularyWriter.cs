using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class VocabularyWriter : IDatabaseWriter<Vocabulary>
{
    public static DatabaseWriter<Vocabulary> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Vocabulary>(SingleIdWriter.CreateSingleIdCommand("locatable", connection));
    }
}
