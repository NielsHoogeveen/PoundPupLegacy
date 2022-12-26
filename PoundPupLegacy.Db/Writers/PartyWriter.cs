using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class PartyWriter : IDatabaseWriter<Party>
{
    public static DatabaseWriter<Party> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Party>(SingleIdWriter.CreateSingleIdCommand("party", connection));
    }
}
