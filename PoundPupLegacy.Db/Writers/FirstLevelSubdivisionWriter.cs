using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class FirstLevelSubdivisionWriter : IDatabaseWriter<FirstLevelSubdivision>
{
    public static DatabaseWriter<FirstLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FirstLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("first_level_subdivision", connection));
    }
}
