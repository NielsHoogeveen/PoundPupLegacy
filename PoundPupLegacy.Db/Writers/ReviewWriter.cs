namespace PoundPupLegacy.Db.Writers;

internal sealed class ReviewWriter : IDatabaseWriter<Review>
{
    public static async Task<DatabaseWriter<Review>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Review>(await SingleIdWriter.CreateSingleIdCommandAsync("review", connection));
    }
}
