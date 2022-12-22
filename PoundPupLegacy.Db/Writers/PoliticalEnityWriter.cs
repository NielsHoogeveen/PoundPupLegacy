using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class PoliticalEnityWriter : IDatabaseWriter<PoliticalEntity>
{
    public static DatabaseWriter<PoliticalEntity> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<PoliticalEntity>(SingleIdWriter.CreateSingleIdCommand("political_entity", connection));
    }
}
