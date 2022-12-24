using Npgsql;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class FirstAndBottomLevelSubdivisionWriter : IDatabaseWriter<FirstAndBottomLevelSubdivision>
{
    public static DatabaseWriter<FirstAndBottomLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FirstAndBottomLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("first_and_bottom_level_subdivision", connection));
    }
}