using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class SecondLevelSubdivisionWriter : IDatabaseWriter<SecondLevelSubdivision>
{
    public static DatabaseWriter<SecondLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<SecondLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("second_level_subdivision", connection));
    }
}
