namespace PoundPupLegacy.Db.Writers;

internal sealed class DenominationWriter : IDatabaseWriter<Denomination>
{
    public static async Task<DatabaseWriter<Denomination>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Denomination>(await SingleIdWriter.CreateSingleIdCommandAsync("denomination", connection));
    }
}
