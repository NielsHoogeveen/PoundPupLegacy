namespace PoundPupLegacy.ViewModel.Readers;

using Request = BlogsDocumentReaderRequest;
using PoundPupLegacy.ViewModel.Models;

public sealed record BlogsDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
}

internal sealed class BlogsDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, List<BlogListEntry>>
{
    internal static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    internal static readonly FieldValueReader<List<BlogListEntry>> DocumentReader = new() { Name = "document" };

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
                    max(tn.url_id) "LatestEntryId",
                    (
                        select 
                            n2.title 
                        from node n2 
                        JOIN tenant_node tn2 on tn2.node_id = n2.id AND tn2.tenant_id = @tenant_id
                        where tn2.url_id = max( tn.url_id)
                    ) "LatestEntryTitle"
                from publisher p
                left join "user" u on u.id = p.id
                join node n on n.publisher_id = p.id 
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
