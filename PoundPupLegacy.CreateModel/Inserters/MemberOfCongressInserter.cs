namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class MemberOfCongressInserter : IDatabaseInserter<MemberOfCongress>
{
    public static async Task<DatabaseInserter<MemberOfCongress>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<MemberOfCongress>("member_of_congress", connection);
    }
}
