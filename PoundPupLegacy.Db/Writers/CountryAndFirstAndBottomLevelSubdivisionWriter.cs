using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class CountryAndFirstAndBottomLevelSubdivisionWriter : IDatabaseWriter<CountryAndFirstAndBottomLevelSubdivision>
{
    public static DatabaseWriter<CountryAndFirstAndBottomLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<CountryAndFirstAndBottomLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("country_and_first_and_bottom_level_subdivision", connection));
    }
}
