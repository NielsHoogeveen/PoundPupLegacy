using Npgsql;
using PoundPupLegacy.ViewModel;
using System.Data;
using System.Security.Claims;

namespace PoundPupLegacy.Services;

public class FetchNodeService
{
    private readonly NpgsqlConnection _connection;
    private readonly StringToDocumentService _stringToDocumentService;
    private readonly SiteDataService _siteDateService;
    public FetchNodeService(NpgsqlConnection connection, StringToDocumentService stringToDocumentService, SiteDataService siteDataService)
    {
        _connection = connection;
        _stringToDocumentService = stringToDocumentService;
        _siteDateService = siteDataService;
    }

    public async Task<Node?> FetchNode(int id, ClaimsPrincipal? claimsPrincipal)
    {
        _connection.Open();
        var sql = $"""
            WITH 
            {AUTHENTICATED_NODE},
            {FETCH_SIMPLE_TEXT_NODE},
            {FETCH_SEE_ALSO_POSTS},
            {FETCH_SEE_ALSO_DOCUMENT},
            {FETCH_TAGS_DOCUMENT},
            {FETCH_COMMENT_DOCUMENT},
            {FETCH_BLOG_POST_BREADCRUM},
            {FETCH_BLOG_POST_DOCUMENT},
            {FETCH_ARTICLE_BREADCRUM},
            {FETCH_ARTICLE_DOCUMENT},
            {FETCH_DOCUMENT}
            SELECT node_type_id, document from fetch_document
            """;

        using var readCommand = _connection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;
        readCommand.Parameters.Add("url_id", NpgsqlTypes.NpgsqlDbType.Integer);
        readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        readCommand.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await readCommand.PrepareAsync();
        readCommand.Parameters["url_id"].Value = id;
        readCommand.Parameters["tenant_id"].Value = 1;
        readCommand.Parameters["user_id"].Value = _siteDateService.GetUserId(claimsPrincipal);
        await using var reader = await readCommand.ExecuteReaderAsync();
        await reader.ReadAsync();
        if (!reader.HasRows)
        {
            return null;
        }
        var node_type_id = reader.GetInt32(0);
        Node node = node_type_id switch
        {
            35 => reader.GetFieldValue<BlogPost>(1),
            36 => reader.GetFieldValue<Article>(1),
            37 => reader.GetFieldValue<Discussion>(1),
            _ => throw new Exception($"Node {id} has Unsupported type {node_type_id}")
        };
        if(node is SimpleTextNode stn)
        {
            stn.Text = _stringToDocumentService.Convert(((SimpleTextNode)node).Text).DocumentNode.InnerHtml;
        }
        _connection.Close();
        return node!;
    }

    const string AUTHENTICATED_NODE = """
        authenticated_node as (
            select
                id,
                title,
                node_type_id,
                tenant_id,
                node_id,
                publisher_id,
                created_date_time,
                changed_date_time,
                url_id,
                url_path,
                subgroup_id,
                publication_status_id,
                case 
                    when status = 0 then false
                    else true
                end has_been_published
            from(
                select
                tn.id,
                n.title,
                n.node_type_id,
                tn.tenant_id,
                tn.node_id,
                n.publisher_id,
                n.created_date_time,
                n.changed_date_time,
                tn.url_id,
                case 
                    when tn.url_path is null then '/node/' || tn.url_id
                    else '/' || url_path
                end url_path,
                tn.subgroup_id,
                tn.publication_status_id,
                case
                    when tn.publication_status_id = 0 then (
                        select
                            case 
                                when count(*) > 0 then 0
                                else -1
                            end status
                        from user_group_user_role_user ugu
                        WHERE ugu.user_group_id = 
                        case
                            when tn.subgroup_id is null then tn.tenant_id 
                            else tn.subgroup_id 
                        end 
                        AND ugu.user_role_id = 6
                        AND ugu.user_id = @user_id
                    )
                    when tn.publication_status_id = 1 then 1
                    when tn.publication_status_id = 2 then (
                        select
                            case 
                                when count(*) > 0 then 1
                                else -1
                            end status
                        from user_group_user_role_user ugu
                        WHERE ugu.user_group_id = 
                            case
                                when tn.subgroup_id is null then tn.tenant_id 
                                else tn.subgroup_id 
                            end
                            AND ugu.user_id = @user_id
                        )
                    end status	
                    from
                    tenant_node tn
                    join node n on n.id = tn.node_id
                    WHERE tn.tenant_id = @tenant_id AND tn.url_id = @url_id
                ) an
                where an.status <> -1
        )
        """;
    const string FETCH_SIMPLE_TEXT_NODE = """
        fetch_simple_text_node AS(
            SELECT
                an.url_id, 
                an.title, 
                an.created_date_time, 
                an.changed_date_time, 
                stn.text, 
                an.publisher_id, 
                p.name publisher_name,
                an.has_been_published
            FROM authenticated_node an
            join simple_text_node stn on stn.id = an.node_id 
            JOIN public.principal p on p.id = an.publisher_id
        )
        """;

    const string FETCH_SEE_ALSO_POSTS = """
        fetch_see_also_posts AS (
         SELECT 
            case 
                when tn.url_path is null then '/node/' || tn.url_id
                else '/' || tn.url_path
            end path,
            n2.title
        FROM authenticated_node an
        JOIN node_term nt1 on nt1.node_id = an.node_id
        JOIN node_term nt2 on nt2.term_id = nt1.term_id and nt2.node_id <> nt1.node_id
        JOIN tenant_node tn on tn.node_id = nt2.node_id and tn.tenant_id = @tenant_id and tn.publication_status_id = 1
        JOIN node n2 on n2.id = tn.node_id
        GROUP BY an.node_id, tn.node_id, tn.url_path, tn.url_id, n2.title
        HAVING COUNT(tn.node_id) > 2 
        ORDER BY count(tn.node_id) desc, n2.title
        LIMIT 10
        )
        """;
    const string FETCH_SEE_ALSO_DOCUMENT = """
        fetch_see_also_document AS(
            SELECT
                json_agg(
                    json_build_object(
                        'Path', sa.path,
                        'Name', sa.title
                    )::jsonb
                )::jsonb agg
            FROM fetch_see_also_posts sa
        )
        """;

    const string FETCH_TAGS_DOCUMENT = """
        fetch_tags_document AS (
            SELECT
                json_agg(
                    json_build_object(
                        'Path',  t.path,
                        'Name', t.name
                    )::jsonb
                )::jsonb as agg
            FROM (
                select
                    case 
                        when tn2.url_path is null then '/node/' || tn2.url_id
                        else '/' || tn2.url_path
                    end path,
                    t.name
                FROM node_term nt 
                JOIN tenant_node tn on tn.node_id = nt.node_id
                JOIN term t on t.id = nt.term_id
                JOIN tenant_node tn2 on tn2.node_id = t.nameable_id and tn2.tenant_id = @tenant_id and tn2.publication_status_id = 1
                WHERE tn.url_id = @url_id and tn.tenant_id = @tenant_id and tn.publication_status_id = 1
            ) t
        )
        """;

    const string FETCH_BLOG_POST_BREADCRUM = """
        fetch_blog_post_bread_crum AS (
            SELECT json_agg(json_build_object(
                'Url', url,
                'Name', "name"
                ) 
            ) bc
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
                FROM authenticated_node an
                JOIN principal p on p.id = an.publisher_id
                WHERE an.url_id = @url_id and an.tenant_id = @tenant_id
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string FETCH_ARTICLE_BREADCRUM = """
        fetch_article_bread_crum AS (
            SELECT json_agg(json_build_object(
                'Url', url,
                'Name', "name"
                ) 
            ) bc
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
                    '/articles', 
                    'aricles', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string FETCH_COMMENT_DOCUMENT = """
        fetch_comments_document AS (
            SELECT json_agg(tree) agg
            FROM (
                SELECT to_jsonb(sub) AS tree
                FROM (
        	        SELECT 
        		        c.id AS "Id", 
        		        c.node_status_id AS "NodeStatusId",
        		        json_build_object(
        			        'Id', p.id, 
        			        'Name', p.name,
                            'CreatedDateTime', c.created_date_time,
                            'ChangedDateTime', c.created_date_time
                        ) AS "Authoring",
        		        c.title AS "Title", 
        		        c.text AS "Text", 
        		        f_comment_tree(c.id) AS "Comments"
        	        FROM comment c
        	        JOIN principal p on p.id = c.publisher_id
                    JOIN authenticated_node an on an.node_id = c.node_id
        	        WHERE an.url_id = @url_id and an.tenant_id = @tenant_id
        	        AND c.comment_id_parent is null
                ) sub
        	) agg        
        )
        """;

    const string FETCH_BLOG_POST_DOCUMENT = """
        fetch_blog_post_document AS (
            SELECT 
                json_build_object(
                'Id', n.url_id,
                'Title', n.title, 
                'Text', n.text,
                'HasBeenPublished', n.has_been_published,
                'Authoring', json_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'BreadCrumElements', (SELECT bc FROM fetch_blog_post_bread_crum),
                'Tags', (SELECT agg FROM fetch_tags_document),
                'SeeAlsoBoxElements', (SELECT agg FROM fetch_see_also_document),
                'Comments', (SELECT agg FROM  fetch_comments_document)
            ) :: jsonb document
            FROM fetch_simple_text_node n
        ) 
        """;

    const string FETCH_ARTICLE_DOCUMENT = """
        fetch_article_document AS (
            SELECT 
                json_build_object(
                'Id', n.url_id,
                'Title', n.title, 
                'Text', n.text,
                'HasBeenPublished', n.has_been_published,
                'Authoring', json_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'BreadCrumElements', (SELECT bc FROM fetch_article_bread_crum),
                'Tags', (SELECT agg FROM fetch_tags_document),
                'SeeAlsoBoxElements', (SELECT agg FROM fetch_see_also_document),
                'Comments', (SELECT agg FROM  fetch_comments_document)
                    ) :: jsonb document
            FROM fetch_simple_text_node n
        ) 
        """;

    const string FETCH_DOCUMENT = """
        fetch_document AS (
            SELECT
                an.node_type_id,
                case 
                    when an.node_type_id = 35 then (select document from fetch_blog_post_document)
                    when an.node_type_id = 36 then (select document from fetch_article_document)
                end document
            FROM authenticated_node an 
            WHERE an.url_id = @url_id and an.tenant_id = @tenant_id
        ) 
        """;

}
