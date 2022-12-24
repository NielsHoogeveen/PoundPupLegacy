using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class FirstAndSecondLevelSubdivisionWriter : IDatabaseWriter<FirstAndSecondLevelSubdivision>
{
    public static DatabaseWriter<FirstAndSecondLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FirstAndSecondLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("first_and_second_level_subdivision", connection));
    }
}