namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class BlogPostWriter : IDatabaseWriter<BlogPost>
{
    public static async Task<DatabaseWriter<BlogPost>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<BlogPost>("blog_post", connection);
    }
}
