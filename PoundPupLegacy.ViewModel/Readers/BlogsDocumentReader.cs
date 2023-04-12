using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.ViewModel.Readers;
public class BlogsDocumentReaderFactory : DatabaseReaderFactory<BlogsDocumentReader>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };

    public override string Sql => SQL;

    const string SQL = """
            select 
                jsonb_agg(to_jsonb(b))
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
    public BlogsDocumentReader(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task<List<BlogListEntry>> ReadAsync(int tenantId)
    {
        _command.Parameters["tenant_id"].Value = tenantId;
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var blogs = reader.GetFieldValue<List<BlogListEntry>>(0);
        return blogs!;
    }

}
