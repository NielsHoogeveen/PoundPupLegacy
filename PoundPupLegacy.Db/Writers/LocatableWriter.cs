using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class LocatableWriter : IDatabaseWriter<Locatable>
{
    public static DatabaseWriter<Locatable> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Locatable>(SingleIdWriter.CreateSingleIdCommand("locatable", connection));
    }
}
