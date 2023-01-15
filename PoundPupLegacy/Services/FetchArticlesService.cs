using Npgsql;
using PoundPupLegacy.Model;
using PoundPupLegacy.ViewModel;
using System.Data;

namespace PoundPupLegacy.Services;

public class FetchArticlesService
{
    NpgsqlConnection _connection;
    TeaserService _teaserService;
    public FetchArticlesService(NpgsqlConnection connection, TeaserService teaserService)
    {
        _connection = connection;
        _teaserService = teaserService;
    }
    const string FETCH_TERMS = """
         fetch_terms as (
         select 
        	json_agg(name)
        		FROM(
        			SELECT 
        				tn.url_id "Id",
        				t.name "Name",
                        false "Selected"
        			FROM article a
        			JOIN node n on n.id = a.id 
        			JOIN node_term nt on nt.node_id = n.id
        			JOIN term t on t.id = nt.term_id
                    JOIN tenant_node tn on tn.node_id = t.nameable_id AND tn.tenant_id = @tenant_id AND tn.publication_status_id = 1 
        			group by 
        				tn.url_id,
        				t.name
        			order by count(a.id) desc, t.name
        			LIMIT 15
        		) name
        	 )
        """;
    const string FETCH_TERMS_FILTERED = """
         fetch_terms as (
         select 
        	json_agg(name)
        		FROM(
        			SELECT 
        				tn.url_id "Id",
        				t.name "Name",
                        CASE
                            WHEN tn.url_id in ({0}) THEN true
                            ELSE false
                        END "Selected"
        			FROM article a
        			JOIN tenant_node tn2 on tn2.node_id = a.id AND tn2.tenant_id = @tenant_id AND tn2.publication_status_id = 1 
        			JOIN node_term nt on nt.node_id = a.id
        			JOIN term t on t.id = nt.term_id
                    JOIN tenant_node tn on tn.node_id = t.nameable_id AND tn.tenant_id = @tenant_id AND tn.publication_status_id = 1 
                    AND a.id in (
                        SELECT
                            n.id
                        FROM node n
                        join article a on a.id = n.id
                        JOIN tenant_node tn2 on tn2.node_id = a.id AND tn2.tenant_id = @tenant_id AND tn2.publication_status_id = 1 
                        join tenant_node tn on tn.url_id in ({0}) AND tn.tenant_id = @tenant_id AND tn.publication_status_id = 1 
                        join term t on t.nameable_id = tn.node_id
        	            left join node_term nt on nt.node_id = n.id and nt.term_id = t.id
        	            GROUP BY
        	                n.id
                        HAVING COUNT(n.id) = COUNT(nt.node_id)
                    )
        			group by 
        				tn.url_id,
        				t.name
        			order by count(a.id) desc, t.name
        			LIMIT 15
        		) name
        	 )
        """;

    const string FETCH_ARTICLES = """
        fetch_articles as (	
        	 select
        	 tn.url_id "Id",
        	 n.title "Title",
             stn.teaser "Text",
        	 json_build_object(
        	    'Id', p.id,
        		'Name', p.name,
        		'CreatedDateTime', n.created_date_time,
        		'ChangedDateTime', n.changed_date_time 
        	 ) "Authoring",
             (select json_agg(json_build_object(
                    'Id', tn.url_id,
                    'Name', t.name
                ))
                FROM node_term nt
                JOIN term t on t.id = nt.term_id
                JOIN tenant_node tn on tn.node_id = t.nameable_id AND tn.tenant_id = @tenant_id AND tn.publication_status_id = 1 
                WHERE nt.node_id = n.id
             ) "Tags"
        	 FROM node n
        	 join article a on a.id = n.id
             join simple_text_node stn on stn.id = n.id
        	 join principal p on p.id = n.publisher_id
             JOIN tenant_node tn on tn.node_id = n.id AND tn.tenant_id = @tenant_id AND tn.publication_status_id = 1 
        	 ORDER BY n.changed_date_time DESC
        	 LIMIT 10
        	)
        """;
    const string FETCH_ARTICLES_FILTERED = """
        fetch_articles as (	
            select
            tn.url_id "Id",
            n.title "Title",
            stn.teaser "Text",
            json_build_object(
                'Id', p.id,
                'Name', p.name,
                'CreatedDateTime', n.created_date_time,
                'ChangedDateTime', n.changed_date_time 
            ) "Authoring",
            (select json_agg(json_build_object(
                    'Id', tn.url_id,
                    'Name', t.name
                ))
                FROM node_term nt
                JOIN term t on t.id = nt.term_id
                JOIN tenant_node tn on tn.node_id = t.nameable_id AND tn.tenant_id = @tenant_id AND tn.publication_status_id = 1 
                WHERE nt.node_id = n.id
             ) "Tags"
            FROM node n
            JOIN tenant_node tn on tn.node_id = n.id AND tn.tenant_id = @tenant_id AND tn.publication_status_id = 1 
            join article a on a.id = n.id
            join simple_text_node stn on stn.id = n.id
            join principal p on p.id = n.publisher_id
            JOIN tenant_node tn2 on tn2.url_id in ({0}) AND tn2.tenant_id = @tenant_id AND tn2.publication_status_id = 1 
        	join term t on t.nameable_id = tn2.node_id
        	left join node_term nt on nt.node_id = n.id and nt.term_id = t.id
        	GROUP BY
            tn.url_id,
        	n.id,
            n.title,
            stn.teaser,
            p.id,
            p.name,
            n.created_date_time,
            n.changed_date_time 
            HAVING COUNT(n.id) = COUNT(nt.node_id)
            ORDER BY n.changed_date_time DESC
            LIMIT 10
        )
        """;
    const string FETCHS_ARTICLES_DOCUMENTS = """
        fetch_articles_document as(
        	select json_agg(to_jsonb(x)) agg
        	from fetch_articles x
        )
        """;

    public async Task<Articles> GetArticles(List<int> selectedTerms)
    {
        var termsList = string.Join(',', selectedTerms.Select(x => x.ToString()));
        _connection.Open();
        var sql = $"""
            WITH
            {string.Format(FETCH_TERMS_FILTERED, termsList)},
            {string.Format(FETCH_ARTICLES_FILTERED, termsList)},
            {FETCHS_ARTICLES_DOCUMENTS}
            select to_jsonb(ta)
            from(
            select 
            	(select * from fetch_terms)  "TermNames",
            	(select agg from fetch_articles_document) "ArticleListEntries"
            	) ta
            """;

        using var readCommand = _connection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;
        readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await readCommand.PrepareAsync();
        readCommand.Parameters["tenant_id"].Value = 1;
        await using var reader = await readCommand.ExecuteReaderAsync();
        await reader.ReadAsync();
        var articles = reader.GetFieldValue<Articles>(0);
        _connection.Close();
        return articles;
    }

    public async Task<Articles> GetArticles()
    {
        _connection.Open();
        var sql = $"""
            WITH
            {FETCH_TERMS},
            {FETCH_ARTICLES},
            {FETCHS_ARTICLES_DOCUMENTS}
            select to_jsonb(ta)
            from(
            select 
            	(select * from fetch_terms)  "TermNames",
            	(select agg from fetch_articles_document) "ArticleListEntries"
            	) ta
            """;

        using var readCommand = _connection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;
        readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await readCommand.PrepareAsync();
        readCommand.Parameters["tenant_id"].Value = 1;
        await using var reader = await readCommand.ExecuteReaderAsync();
        await reader.ReadAsync();
        var articles = reader.GetFieldValue<Articles>(0);
        _connection.Close();
        return articles;
    }
}
