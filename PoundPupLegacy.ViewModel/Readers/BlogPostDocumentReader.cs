namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = NodeDocumentReaderRequest;

internal sealed class BlogPostDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, BlogPost>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeIdParameter = new() { Name = "node_id" };

    public override string Sql => SQL;
    const string SQL = $"""
            {SharedViewerSql.SIMPLE_TEXT_NODE_WITH_SEE_ALSO_BOX},
            {BREADCRUM_DOCUMENT},
            {DOCUMENT}
            SELECT document from blog_post_document
            """
            ;

    const string DOCUMENT = """
        blog_post_document AS (
            SELECT 
                jsonb_build_object(
                'NodeId', 
                node_id,
                'NodeTypeId', 
                node_type_id,
                'Title', 
                title, 
                'Text', 
                text,
                'HasBeenPublished', 
                has_been_published,
                'PublicationStatusId', 
                publication_status_id,
                'Authoring', 
                jsonb_build_object(
                    'Id', publisher_id, 
                    'Name', publisher_name,
                    'CreatedDateTime', created_date_time,
                    'ChangedDateTime', changed_date_time
                ),
                'BreadCrumElements', 
                (SELECT document FROM bread_crum_document),
                'Tags', 
                (SELECT document FROM tags_document),
                'SeeAlsoBoxElements', 
                (SELECT document FROM see_also_document),
                'CommentListItems', 
                (SELECT document FROM  comments_document),
                'Files', 
                (SELECT document FROM files_document)
            ) document
            FROM (
                SELECT
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
                    end has_been_published,
                    tn.publication_status_id
                FROM node n
                join tenant_node tn on tn.node_id = n.id 
                join simple_text_node stn on stn.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                where tn.node_id = @node_id and tn.tenant_id = @tenant_id
                and tn.publication_status_id in 
                (
                    select 
                    publication_status_id 
                    from user_publication_status 
                    where tenant_id = tn.tenant_id 
                    and user_id = @user_id
                    and (
                        subgroup_id = tn.subgroup_id 
                        or subgroup_id is null and tn.subgroup_id is null
                    )
                )
        
            ) n
        ) 
        """;


    const string BREADCRUM_DOCUMENT = """
        bread_crum_document AS (
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
                    '/' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/blogs', 
                    'blogs', 
                    1
                UNION
                SELECT 
                    '/blog/' || p.id, 
                    p.name || '''s blog', 
                    2
                FROM node n
                join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id and tn.node_id = @node_id
                JOIN publisher p on p.id = n.publisher_id
                WHERE tn.node_id = @node_id and tn.tenant_id = @tenant_id
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;



    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeIdParameter, request.NodeId),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
        };
    }

    protected override BlogPost? Read(NpgsqlDataReader reader)
    {
        return reader.GetFieldValue<BlogPost>(0);
    }
}
