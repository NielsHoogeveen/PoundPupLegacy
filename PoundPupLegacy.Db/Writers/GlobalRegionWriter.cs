using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class GlobalRegionWriter : IDatabaseWriter<GlobalRegion>
{
    public static DatabaseWriter<GlobalRegion> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<GlobalRegion>(SingleIdWriter.CreateSingleIdCommand("global_region", connection));
    }
}
