using Npgsql;
using PoundPupLegacy.EditModel;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

public class EditorService : IEditorService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISiteDataService _siteDateService;
    private readonly INodeCacheService _nodeCacheService;
    private readonly ITextService _textService;
    public EditorService(
    NpgsqlConnection connection,
    ISiteDataService siteDataService,
    INodeCacheService nodeCacheService,
    ITextService textService)
    {
        _connection = connection;
        _siteDateService = siteDataService;
        _nodeCacheService = nodeCacheService;
        _textService = textService;
    }
    public async Task<BlogPost?> GetBlogPost(int id)
    {
        try
        {
            await _connection.OpenAsync();
            var sql = $"""
            select
                jsonb_build_object(
                    'NodeId', n.id,
                    'UrlId', tn.url_id,
                    'Title' , n.title,
                    'Text', stn.text
            	) document
            from node n
            join blog_post b on b.id = n.id
            join simple_text_node stn on stn.id = n.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            """;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("url_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await readCommand.PrepareAsync();
            readCommand.Parameters["url_id"].Value = id;
            readCommand.Parameters["tenant_id"].Value = _siteDateService.GetTenantId();
            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            if (!reader.HasRows)
            {
                return null;
            }
            return reader.GetFieldValue<BlogPost>(0);
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task Save(BlogPost post)
    {
        try
        {
            await _connection.OpenAsync();
            var sql = $"""
            update node set title=@title
            where id = @id;
            update simple_text_node set text=@text, teaser=@teaser
            where id = @id;;
            """;

            using var command = _connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 300;
            command.CommandText = sql;
            command.Parameters.Add("text", NpgsqlTypes.NpgsqlDbType.Varchar);
            command.Parameters.Add("teaser", NpgsqlTypes.NpgsqlDbType.Varchar);
            command.Parameters.Add("title", NpgsqlTypes.NpgsqlDbType.Varchar);
            command.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            command.Parameters["title"].Value = post.Title;
            command.Parameters["text"].Value = _textService.FormatText(post.Text);
            command.Parameters["teaser"].Value = _textService.FormatTeaser(post.Text);
            command.Parameters["id"].Value = post.NodeId;
            var u = await command.ExecuteNonQueryAsync();
            _nodeCacheService.Remove(post.UrlId);
        }
        finally
        {
            await _connection.CloseAsync();
        }

    }
}
