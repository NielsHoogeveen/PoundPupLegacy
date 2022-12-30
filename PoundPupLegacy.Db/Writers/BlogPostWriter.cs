using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class BlogPostWriter : DatabaseWriter<BlogPost>, IDatabaseWriter<BlogPost>
{
    private const string ID = "id";
    private const string TEXT = "text";
    public static DatabaseWriter<BlogPost> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
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

    internal override void Write(BlogPost blogPost)
    {
        try
        {
            WriteValue(blogPost.Id, ID);
            WriteValue(blogPost.Text, TEXT);
            _command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
