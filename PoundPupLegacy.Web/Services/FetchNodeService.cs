using Npgsql;
using PoundPupLegacy.ViewModel;
using System.Data;

namespace PoundPupLegacy.Web.Services;

public class FetchNodeService
{
    private NpgsqlConnection _connection;
    public FetchNodeService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Node> FetchNode(int id)
    {
        _connection.Open();
        var sql = $"""
            WITH 
            {FETCH_BLOG_POSTS},
            {FETCH_SEE_ALSO_POSTS},
            {FETCH_SEE_ALSO_DOCUMENT},
            {FETCH_TAGS_DOCUMENT},
            {FETCH_COMMENT_DOCUMENT},
            {FETCH_BLOG_POST_BREADCRUM},
            {FETCH_BLOG_POST_DOCUMENT},
            {FETCH_DOCUMENT}
            SELECT node_type_id, document from fetch_document
            """;

        using var readCommand = _connection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;
        readCommand.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await readCommand.PrepareAsync();
        readCommand.Parameters["node_id"].Value = id;
        await using var reader = await readCommand.ExecuteReaderAsync();
        await reader.ReadAsync();
        var node_type_id = reader.GetInt32(0);
        var node = node_type_id switch
        {
            35 => reader.GetFieldValue<BlogPost>(1),
            _ => throw new Exception($"Node {id} has Unsupported type {node_type_id}")
        };
        _connection.Close();
        return node!;
    }
    const string FETCH_BLOG_POSTS = """
        fetch_blog_post AS(
            SELECT
                n.id, 
                n.title, 
                n.created_date_time, 
                n.changed_date_time, 
                stn.text, 
                n.access_role_id, 
                ar.name access_role_name
            FROM public."node" n
            JOIN public."blog_post" bp on bp.id = n.id
            JOIN public."simple_text_node" stn on stn.id = n.id
            JOIN public."access_role" ar on ar.id = n.access_role_id
            WHERE n.id = @node_id
        )
        """;

    const string FETCH_SEE_ALSO_POSTS = """
        fetch_see_also_posts AS (
         SELECT 
            n.id node_id,
            n2.id node_id_other,
            n2.title node_title_other,
            count(n2.id)
        FROM node n
        JOIN node_term nt1 on nt1.node_id = n.id
        JOIN node_term nt2 on nt2.term_id = nt1.term_id and nt2.node_id <> nt1.node_id
        JOIN term t on t.id = nt1.term_id
        JOIN node n2 on n2.id = nt2.node_id AND n2.node_status_id = 1
        WHERE n.id = @node_id
        GROUP BY n.id, n2.id, n2.title
        HAVING COUNT(n2.id) > 2 
        ORDER BY count(n2.id) desc, n2.title
        LIMIT 10
        )
        """;
    const string FETCH_SEE_ALSO_DOCUMENT = """
        fetch_see_also_document AS(
            SELECT
                json_agg(
                    json_build_object(
                        'Id', sa.node_id_other,
                        'Name', sa.node_title_other
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
                        'Id', t.nameable_id,
                        'Name', t.name
                    )::jsonb
                )::jsonb as agg
            FROM node_term nt 
            JOIN term t on t.id = nt.term_id
            WHERE nt.node_id = @node_id 
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
                    '/blog+/' || ar.id, 
                    ar.name || '''s blog', 
                    2
                FROM node n
                JOIN access_role ar on ar.id = n.access_role_id
                WHERE n.id = 38199
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
        			        'Id', ar.id, 
        			        'Name', ar.name,
                            'CreatedDateTime', c.created_date_time,
                            'ChangedDateTime', c.created_date_time
                        ) AS "Authoring",
        		        c.title AS "Title", 
        		        c.text AS "Text", 
        		        f_comment_tree(c.id) AS "Comments"
        	        FROM comment c
        	        JOIN access_role ar on ar.id = c.access_role_id
        	        WHERE c.node_id = @node_id
        	        AND c.comment_id_parent is null
                ) sub
        	) agg        
        )
        """;

    const string FETCH_BLOG_POST_DOCUMENT = """
        fetch_blog_post_document AS (
            SELECT 
                json_build_object(
                'Id', n.id,
                'Title', n.title, 
                'Text', n.text,
                'Authoring', json_build_object(
                    'Id', n.access_role_id, 
                    'Name', n.access_role_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'BreadCrumElements', (SELECT bc FROM fetch_blog_post_bread_crum),
                'Tags', (SELECT agg FROM fetch_tags_document),
                'SeeAlsoBoxElements', (SELECT agg FROM fetch_see_also_document),
                'Comments', (SELECT agg FROM  fetch_comments_document)
            ) :: jsonb document
            FROM fetch_blog_post n
        ) 
        """;

    const string FETCH_DOCUMENT = """
        fetch_document AS (
            SELECT
                n.node_type_id,
                case 
                    when n.node_type_id = 35 then (select document from fetch_blog_post_document)
                end document
            FROM node n 
            WHERE n.id = @node_id
        ) 
        """;

}
