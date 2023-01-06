namespace PoundPupLegacy.Db.Writers;

internal class BlogPostWriter : IDatabaseWriter<BlogPost>
{
    public static async Task<DatabaseWriter<BlogPost>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<BlogPost>(await SingleIdWriter.CreateSingleIdCommandAsync("blog_post", connection));
    }
}
