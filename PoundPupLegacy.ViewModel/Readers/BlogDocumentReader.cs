namespace PoundPupLegacy.ViewModel.Readers;

using Request = BlogDocumentReaderRequest;
using PoundPupLegacy.ViewModel.Models;

public sealed record BlogDocumentReaderRequest : IRequest
{
    public required int PublisherId { get; init; }
    public required int TenantId { get; init; }
    public required int Length { get; init; }
    public required int StartIndex { get; init; }
}

internal sealed class BlogDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, Blog>
{
    internal static readonly NonNullableIntegerDatabaseParameter PublishedIdParameter = new() { Name = "publisher_id" };
    internal static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    internal static readonly NonNullableIntegerDatabaseParameter LengthParameter = new() { Name = "length" };
    internal static readonly NonNullableIntegerDatabaseParameter StartIndexParameter = new() { Name = "start_index" };

    internal static readonly FieldValueReader<Blog> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
            WITH 
            fetch_blog_post_documents AS (
                SELECT 
                    jsonb_build_object(
                    'Id', 
                    n.id,
                    'Path',
                    case 
                        when url_path is null then '/node/' || n.id
                        else '/path/' || url_path
                    end,
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
                        tn.url_path url_path,
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
                    'Entries', 
                    (select jsonb_agg(document) from fetch_blog_post_documents)
                ) document
                FROM publisher p
                JOIN node n on n.publisher_id = p.id
                JOIN blog_post b on b.id = n.id
                JOIN tenant_node tn on tn.node_id = n.id AND tn.tenant_id = @tenant_id
                WHERE p.id = @publisher_id AND tn.publication_status_id = 1
                GROUP BY p.id, p.name
            """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PublishedIdParameter, request.PublisherId),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(StartIndexParameter, request.StartIndex),
            ParameterValue.Create(LengthParameter, request.Length),
        };
    }

    protected override Blog Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
