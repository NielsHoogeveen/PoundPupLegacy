namespace PoundPupLegacy.Db.Writers;

internal sealed class MemberOfCongressWriter : IDatabaseWriter<MemberOfCongress>
{
    public static async Task<DatabaseWriter<MemberOfCongress>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<MemberOfCongress>("member_of_congress", connection);
    }
}
