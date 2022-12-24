using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class CountryAndFirstAndSecondLevelSubdivisionWriter : IDatabaseWriter<CountryAndFirstAndSecondLevelSubdivision>
{
    public static DatabaseWriter<CountryAndFirstAndSecondLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<CountryAndFirstAndSecondLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("country_and_first_and_second_level_subdivision", connection));
    }
}
