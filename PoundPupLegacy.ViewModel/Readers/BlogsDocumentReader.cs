namespace PoundPupLegacy.ViewModel.Readers;

using Factory = BlogsDocumentReaderFactory;
using Reader = BlogsDocumentReader;

public class BlogsDocumentReaderFactory : DatabaseReaderFactory<Reader>
{
    internal static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    internal static readonly FieldValueReader<List<BlogListEntry>> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = """
            select 
                jsonb_agg(to_jsonb(b)) document
            from(
                select 
                    p.name "Name",
                    p.id "Id",
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


}
public class BlogsDocumentReader : SingleItemDatabaseReader<int, List<BlogListEntry>>
{
    internal BlogsDocumentReader(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(int request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.TenantIdParameter, request),
        };
    }
    protected override List<BlogListEntry> Read(NpgsqlDataReader reader)
    {
        return Factory.DocumentReader.GetValue(reader);
    }
}
