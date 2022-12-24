using Npgsql;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class InformalIntermediateLevelSubdivisionWriter : IDatabaseWriter<InformalIntermediateLevelSubdivision>
{
    public static DatabaseWriter<InformalIntermediateLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<InformalIntermediateLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("informal_intermediate_level_subdivision", connection));
    }
}