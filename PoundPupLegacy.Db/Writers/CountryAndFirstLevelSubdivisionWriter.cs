using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class CountryAndFirstLevelSubdivisionWriter : IDatabaseWriter<CountryAndFirstLevelSubdivision>
{
    public static DatabaseWriter<CountryAndFirstLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<CountryAndFirstLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("country_and_first_level_subdivision", connection));
    }
}
