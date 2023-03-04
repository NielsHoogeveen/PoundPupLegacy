namespace PoundPupLegacy.Db;

public class AdoptionExportYearCreator : IEntityCreator<AdoptionExportYear>
{
    public static async Task CreateAsync(IAsyncEnumerable<AdoptionExportYear> adoptionExportYears, NpgsqlConnection connection)
    {

        await using var adoptionExportYearWriter = await AdoptionExportYearWriter.CreateAsync(connection);

        await foreach (var adoptionExportYear in adoptionExportYears) {
            await adoptionExportYearWriter.WriteAsync(adoptionExportYear);
        }
    }
}
