namespace PoundPupLegacy.Db.Writers;

internal class BlogPostWriter : DatabaseWriter<BlogPost>, IDatabaseWriter<BlogPost>
{
    private const string ID = "id";
    private const string TEXT = "text";
    public static async Task<DatabaseWriter<BlogPost>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "blog_post",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TEXT,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new BlogPostWriter(command);

    }

    internal BlogPostWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(BlogPost blogPost)
    {
        if (blogPost.Id is null)
            throw new NullReferenceException();

        WriteValue(blogPost.Id, ID);
        WriteValue(blogPost.Text, TEXT);
        await _command.ExecuteNonQueryAsync();
    }
}
