using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class CountryAndIntermediateLevelSubdivisionWriter : IDatabaseWriter<CountryAndIntermediateLevelSubdivision>
{
    public static DatabaseWriter<CountryAndIntermediateLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<CountryAndIntermediateLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("country_and_intermediate_level_subdivision", connection));
    }
}
