namespace PoundPupLegacy.ViewModel.Readers;
using PoundPupLegacy.ViewModel.Models;
using Request = NodeTypeReaderRequest;

internal sealed class BlogPostReaderFactory : SingleItemDatabaseReaderFactory<Request, BlogPost>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter UrlIdParameter = new() { Name = "url_id" };

    public override string Sql => SQL;

    const string SQL = $"""
        {SharedViewerSql.NODE_DOCUMENT}
        {BLOG_POST_BREADCRUM_DOCUMENT},
        {BLOG_POST_DOCUMENT}
        select document from blog_post_document
        """;
        //{SharedViewerSql.SEE_ALSO_DOCUMENT},
        //{SharedViewerSql.COMMENTS_DOCUMENT},

    const string BLOG_POST_BREADCRUM_DOCUMENT = """
        blog_post_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Title', "name"
                ) 
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/blogs', 
                    'blogs', 
                    1
                UNION
                SELECT 
                    '/blog+/' || p.id, 
                    p.name || '''s blog', 
                    2
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                JOIN publisher p on p.id = n.publisher_id
                WHERE tn.url_id = @url_id and tn.tenant_id = @tenant_id
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;


    const string BLOG_POST_DOCUMENT = """
        blog_post_document AS (
            SELECT 
            jsonb_build_object(
                'UrlId', n.url_id,
                'NodeId', n.node_id,
                'NodeTypeId', n.node_type_id,
                'Title', n.title, 
                'Text', n.text,
                'HasBeenPublished', n.has_been_published,
                'Authoring', jsonb_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'BreadCrumElements', (SELECT document FROM blog_post_bread_crum_document),
                'Tags', 
                --(SELECT document FROM tags_document),
                null,
                'SeeAlsoBoxElements', 
                --(SELECT document FROM see_also_document),
                null,
                'CommentListItems', 
                --(SELECT document FROM  comments_document),
                null,
                'Files', 
                --(SELECT document FROM files_document)
                null
            ) document
            FROM (
                SELECT
                    tn.url_id,  
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    stn.text, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case 
                        when tn.publication_status_id = 0 then false
                        else true
                    end has_been_published
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join simple_text_node stn on stn.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;


    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UrlIdParameter, request.UrlId),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
        };
    }

    protected override BlogPost? Read(NpgsqlDataReader reader)
    {
        return reader.GetFieldValue<BlogPost>(0);

    }
}
