namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BlogPostInserter : IDatabaseInserter<BlogPost>
{
    public static async Task<DatabaseInserter<BlogPost>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<BlogPost>("blog_post", connection);
    }
}
