namespace PoundPupLegacy.Db.Writers;

internal class DenominationWriter : IDatabaseWriter<Denomination>
{
    public static async Task<DatabaseWriter<Denomination>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Denomination>(await SingleIdWriter.CreateSingleIdCommandAsync("denomination", connection));
    }
}
