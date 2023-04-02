namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class TypeOfAbuserWriter : IDatabaseInserter<TypeOfAbuser>
{
    public static async Task<DatabaseInserter<TypeOfAbuser>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<TypeOfAbuser>("type_of_abuser", connection);
    }
}
