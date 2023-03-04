using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.ViewModel;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchBlogService : IFetchBlogService
{
    private readonly NpgsqlConnection _connection;
    private readonly IRazorViewToStringService _renderer;
    private readonly ISiteDataService _siteDataService;

    public FetchBlogService(NpgsqlConnection connection, IRazorViewToStringService renderer, ISiteDataService siteDataService)
    {
        _connection = connection;
        _renderer = renderer;
        _siteDataService = siteDataService;
    }

    public async Task<Blog> FetchBlog(int publisherId, int startIndex, int length)
    {
        try {
            await _connection.OpenAsync();
            var sql = $"""
            WITH 
            {FETCH_BLOG_POST_DOCUMENT}
            SELECT 
                jsonb_build_object(
                    'Name', p.name,
                    'NumberOfEntries', COUNT(n.id),
                    'BlogPostTeasers', (select jsonb_agg(document) from fetch_blog_post_documents)
                )
                FROM publisher p
                JOIN node n on n.publisher_id = p.id
                JOIN blog_post b on b.id = n.id
                JOIN tenant_node tn on tn.node_id = n.id AND tn.tenant_id = @tenant_id
                WHERE p.id = @publisher_id AND tn.publication_status_id = 1
                GROUP BY p.name
            """;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("publisher_id", NpgsqlDbType.Integer);
            readCommand.Parameters.Add("tenant_id", NpgsqlDbType.Integer);
            readCommand.Parameters.Add("length", NpgsqlDbType.Integer);
            readCommand.Parameters.Add("start_index", NpgsqlDbType.Integer);
            await readCommand.PrepareAsync();
            readCommand.Parameters["publisher_id"].Value = publisherId;
            readCommand.Parameters["tenant_id"].Value = _siteDataService.GetTenantId();
            readCommand.Parameters["length"].Value = length;
            readCommand.Parameters["start_index"].Value = startIndex;
            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            var blog = reader.GetFieldValue<Blog>(0);
            var c = _renderer.GetFromView("/Pages/_ExcecutiveCompensations.cshtml", new List<ExecutiveCompensation>());
            var entries = blog.BlogPostTeasers.Select(x => new BlogPostTeaser {
                Id = x.Id,
                Authoring = x.Authoring,
                Title = x.Title,
                Text = x.Text
            });
            blog.Name = MakeName(blog.Name);
            blog.BlogPostTeasers = entries.ToList();
            return blog!;
        }
        finally {
            await _connection.CloseAsync();
        }
    }
    private static string MakeName(string name)
    {
        if (name.EndsWith("s")) {
            return $"{name}' blog";
        }
        return $"{name}'s blog";
    }



    const string FETCH_BLOG_POST_DOCUMENT = """
        fetch_blog_post_documents AS (
            SELECT 
                jsonb_build_object(
                'Id', n.id,
                'Title', n.title, 
                'Text', n.teaser,
                'Authoring', json_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                )
            ) document
            FROM (
                SELECT
                    tn.url_id id, 
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    stn.teaser,
                    n.publisher_id, 
                    p.name publisher_name
                FROM node n
                JOIN blog_post bp on bp.id = n.id
                JOIN simple_text_node stn on stn.id = n.id
                JOIN publisher p on p.id = n.publisher_id
                JOIN tenant_node tn on tn.node_id = n.id AND tn.tenant_id = @tenant_id
                WHERE p.id = @publisher_id AND tn.publication_status_id = 1
                ORDER BY n.created_date_time DESC
                LIMIT @length OFFSET @start_index
            ) n
        ) 
        """;

}
