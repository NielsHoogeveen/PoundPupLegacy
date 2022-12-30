using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class AdoptionExportYearCreator : IEntityCreator<AdoptionExportYear>
{
    public static void Create(IEnumerable<AdoptionExportYear> adoptionExportYears, NpgsqlConnection connection)
    {

        using var adoptionExportYearWriter = AdoptionExportYearWriter.Create(connection);

        foreach (var adoptionExportYear in adoptionExportYears)
        {
            adoptionExportYearWriter.Write(adoptionExportYear);
        }
    }
}
