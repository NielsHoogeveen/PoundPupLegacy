using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class IntermediateLevelSubdivisionWriter : IDatabaseWriter<IntermediateLevelSubdivision>
{
    public static DatabaseWriter<IntermediateLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<IntermediateLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("intermediate_level_subdivision", connection));
    }
}
