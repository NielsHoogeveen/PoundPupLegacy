using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.ViewModel;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchBlogService : IFetchBlogService
{
    private NpgsqlConnection _connection;

    private IRazorViewToStringService _renderer;

    public FetchBlogService(NpgsqlConnection connection, IRazorViewToStringService renderer)
    {
        _connection = connection;
        _renderer = renderer;
    }

    public async Task<Blog> FetchBlog(HttpContext context, int accessRoleId, int startIndex, int length)
    {
        try
        {
            await _connection.OpenAsync();
            var sql = $"""
            WITH 
            {FETCH_BLOG_POSTS},
            {FETCH_BLOG_POST_DOCUMENT}
            SELECT 
                json_build_object(
                    'Name', ar.name,
                    'NumberOfEntries', COUNT(n.id),
                    'BlogPostTeasers', (select json_agg(document) from fetch_blog_post_documents)
                )
                FROM access_role ar
                JOIN node n on n.access_role_id = ar.id
                JOIN blog_post b on b.id = n.id
                WHERE ar.id = @access_role_id AND n.node_status_id = 1
                GROUP BY ar.name
            """;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("access_role_id", NpgsqlDbType.Integer);
            readCommand.Parameters.Add("length", NpgsqlDbType.Integer);
            readCommand.Parameters.Add("start_index", NpgsqlDbType.Integer);
            await readCommand.PrepareAsync();
            readCommand.Parameters["access_role_id"].Value = accessRoleId;
            readCommand.Parameters["length"].Value = length;
            readCommand.Parameters["start_index"].Value = startIndex;
            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            var blog = reader.GetFieldValue<Blog>(0);
            var c = _renderer.GetFromView("/Pages/_ExcecutiveCompensations.cshtml", new List<ExecutiveCompensation>(), context);
            var entries = blog.BlogPostTeasers.Select(x => new BlogPostTeaser
            {
                Id = x.Id,
                Authoring = x.Authoring,
                Title = x.Title,
                Text = x.Text
            });
            blog.Name = MakeName(blog.Name);
            blog.BlogPostTeasers = entries.ToList();
            return blog!;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }
    private static string MakeName(string name)
    {
        if (name.EndsWith("s"))
        {
            return $"{name}' blog";
        }
        return $"{name}'s blog";
    }

    const string FETCH_BLOG_POSTS = """
        fetch_blog_posts AS(
            SELECT
                n.id, 
                n.title, 
                n.created_date_time, 
                n.changed_date_time, 
                stn.teaser,
                n.access_role_id, 
                ar.name access_role_name
            FROM public."node" n
            JOIN public."blog_post" bp on bp.id = n.id
            JOIN public."simple_text_node" stn on stn.id = n.id
            JOIN public."access_role" ar on ar.id = n.access_role_id
            WHERE ar.id = @access_role_id AND n.node_status_id = 1
            ORDER BY n.changed_date_time DESC
            LIMIT @length OFFSET @start_index
        )
        """;


    const string FETCH_BLOG_POST_DOCUMENT = """
        fetch_blog_post_documents AS (
            SELECT 
                json_build_object(
                'Id', n.id,
                'Title', n.title, 
                'Text', n.teaser,
                'Authoring', json_build_object(
                    'Id', n.access_role_id, 
                    'Name', n.access_role_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                )
            ) :: jsonb document
            FROM fetch_blog_posts n
        ) 
        """;

}
