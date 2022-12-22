using Npgsql;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class CountryWriter : IDatabaseWriter<Country>
{
    public static DatabaseWriter<Country> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Country>(SingleIdWriter.CreateSingleIdCommand("country", connection));
    }
}
