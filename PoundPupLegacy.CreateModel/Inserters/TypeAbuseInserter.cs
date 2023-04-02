namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class TypeOfAbuseWriter : IDatabaseInserter<TypeOfAbuse>
{
    public static async Task<DatabaseInserter<TypeOfAbuse>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<TypeOfAbuse>("type_of_abuse", connection);
    }
}
