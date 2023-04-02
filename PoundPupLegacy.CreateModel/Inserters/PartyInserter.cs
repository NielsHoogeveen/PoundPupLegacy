namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PartyInserter : IDatabaseInserter<Party>
{
    public static async Task<DatabaseInserter<Party>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Party>("party", connection);
    }
}
