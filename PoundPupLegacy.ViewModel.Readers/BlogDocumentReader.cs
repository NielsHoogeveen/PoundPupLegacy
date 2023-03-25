using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Common;
using System.Data.Common;
using System.Data;

namespace PoundPupLegacy.ViewModel.Readers;

public class BlogDocumentReader : DatabaseReader, IDatabaseReader<BlogDocumentReader>
{
    private BlogDocumentReader(NpgsqlCommand command) : base(command)
    {
    }
    public async Task<Blog> ReadAsync(int publisherId, int tenantId, int startIndex, int length)
    {
        string MakeName(string name)
        {
            if (name.EndsWith("s")) {
                return $"{name}' blog";
            }
            return $"{name}'s blog";
        }
        _command.Parameters["publisher_id"].Value = publisherId;
        _command.Parameters["tenant_id"].Value = tenantId;
        _command.Parameters["length"].Value = length;
        _command.Parameters["start_index"].Value = startIndex;
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var blog = reader.GetFieldValue<Blog>(0);
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

    public static async Task<BlogDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        command.Parameters.Add("publisher_id", NpgsqlDbType.Integer);
        command.Parameters.Add("tenant_id", NpgsqlDbType.Integer);
        command.Parameters.Add("length", NpgsqlDbType.Integer);
        command.Parameters.Add("start_index", NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new BlogDocumentReader(command);

    }

    const string SQL = $"""
            WITH 
            fetch_blog_post_documents AS (
                SELECT 
                    jsonb_build_object(
                    'Id', 
                    n.id,
                    'Title', 
                    n.title, 
                    'Text', 
                    n.teaser,
                    'Authoring', 
                    jsonb_build_object(
                        'Id', 
                        n.publisher_id, 
                        'Name', 
                        n.publisher_name,
                        'CreatedDateTime', 
                        n.created_date_time,
                        'ChangedDateTime', 
                        n.changed_date_time
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
            SELECT 
                jsonb_build_object(
                    'Name', 
                    p.name,
                    'NumberOfEntries', 
                    COUNT(n.id),
                    'BlogPostTeasers', 
                    (select jsonb_agg(document) from fetch_blog_post_documents)
                )
                FROM publisher p
                JOIN node n on n.publisher_id = p.id
                JOIN blog_post b on b.id = n.id
                JOIN tenant_node tn on tn.node_id = n.id AND tn.tenant_id = @tenant_id
                WHERE p.id = @publisher_id AND tn.publication_status_id = 1
                GROUP BY p.name
            """;

}
