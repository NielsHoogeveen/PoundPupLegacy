namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = BlogsDocumentReaderRequest;

public sealed record BlogsDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
}

internal sealed class BlogsDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, List<BlogListEntry>>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly FieldValueReader<List<BlogListEntry>> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = """
            select 
                jsonb_agg(to_jsonb(b)) document
            from(
                select 
                    p.name "Title",
                    p.id "Id",
                    '/blog/' || p.id "Path",
                    u.avatar "FilePathAvatar",
                    COUNT(n.id) "NumberOfEntries",
                    (
                        select 
                        jsonb_build_object(
                            'Title', 
                            n2.title,
                            'Path', 
                            '/' || nt2.viewer_path || '/' || tn2.node_id 
                        )
                        from node n2 
                        join node_type nt2 on n2.node_type_id = nt2.id
                        JOIN tenant_node tn2 on tn2.node_id = n2.id AND tn2.tenant_id = @tenant_id
                        where tn2.node_id = max( tn.node_id)
                    ) "LatestEntry"
                from publisher p
                left join "user" u on u.id = p.id
                join node n on n.publisher_id = p.id 
                join node_type nt on n.node_type_id = nt.id
                join tenant_node tn on tn.node_id = n.id and tn.publication_status_id = 1 and tn.tenant_id = @tenant_id
                join blog_post b on b.id = n.id
                group by p.name,
                p.id,
                u.created_date_time,
                u.avatar
                order by COUNT(n.id) desc, u.created_date_time 
            ) b
            """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
        };
    }
    protected override List<BlogListEntry> Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
