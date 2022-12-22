using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class BasicCountryAndFirstLevelSubdivisionWriter : IDatabaseWriter<BasicCountryAndFirstLevelSubdivision>
{
    public static DatabaseWriter<BasicCountryAndFirstLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<BasicCountryAndFirstLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("basic_country_and_first_level_subdivision", connection));
    }
}
