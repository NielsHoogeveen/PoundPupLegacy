using Npgsql;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class FormalIntermediateLevelSubdivisionWriter : IDatabaseWriter<FormalIntermediateLevelSubdivision>
{
    public static DatabaseWriter<FormalIntermediateLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FormalIntermediateLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("formal_intermediate_level_subdivision", connection));
    }
}