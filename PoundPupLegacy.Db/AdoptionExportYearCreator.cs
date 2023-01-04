namespace PoundPupLegacy.Db;

public class AdoptionExportYearCreator : IEntityCreator<AdoptionExportYear>
{
    public static async Task CreateAsync(IEnumerable<AdoptionExportYear> adoptionExportYears, NpgsqlConnection connection)
    {

        await using var adoptionExportYearWriter = await AdoptionExportYearWriter.CreateAsync(connection);

        foreach (var adoptionExportYear in adoptionExportYears)
        {
            await adoptionExportYearWriter.WriteAsync(adoptionExportYear);
        }
    }
}
