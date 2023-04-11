using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.ViewModel.Readers;
public class BlogDocumentReaderFactory : IDatabaseReaderFactory<BlogDocumentReader>
{
    public async Task<BlogDocumentReader> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

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
                    'Id',
                    p.id,
                    'Name', 
                    case
                        when SUBSTRING(p.name, char_length(p.name)) = 's' then p.name || '''' || ' blog'
                        else p.name || '''' || 's blog' 
                    end,
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
                GROUP BY p.id, p.name
            """;

}
public class BlogDocumentReader : SingleItemDatabaseReader<BlogDocumentReader.BlogDocumentRequest, Blog>
{
    public record BlogDocumentRequest
    {
        public int PublisherId { get; init; }
        public int TenantId { get; init; }
        public int StartIndex { get; init; }
        public int Length { get; init; }
    }
    internal BlogDocumentReader(NpgsqlCommand command) : base(command)
    {
    }
    public override async Task<Blog> ReadAsync(BlogDocumentRequest request)
    {
        _command.Parameters["publisher_id"].Value = request.PublisherId;
        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["length"].Value = request.Length;
        _command.Parameters["start_index"].Value = request.StartIndex;
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var blog = reader.GetFieldValue<Blog>(0);
        return blog!;
    }
}
