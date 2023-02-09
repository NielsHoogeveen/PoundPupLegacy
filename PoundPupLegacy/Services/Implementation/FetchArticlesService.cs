using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.ViewModel;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchArticlesService: IFetchArticlesService
{
    NpgsqlConnection _connection;
    
    public FetchArticlesService(NpgsqlConnection connection)
    {
        _connection = connection;
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
                        JOIN tenant_node tna on tna.node_id = n.id AND tna.tenant_id = @tenant_id AND tna.publication_status_id = 1 				
                        join article a on a.id = n.id
                        join tenant te on te.id = @tenant_id
                        join tenant_node tnt on tnt.url_id in ({0}) AND tnt.tenant_id = @tenant_id AND tnt.publication_status_id = 1 
                        JOIN nameable nm on nm.id = tnt.node_id
                        JOIN term t on t.nameable_id = tnt.node_id and t.vocabulary_id = te.vocabulary_id_tagging
                        left join node_term nt on nt.term_id = t.id and nt.node_id = n.id
                        group by n.id, n.title, n.created_date_time
                        having count(t.id) = count(nt.node_id)
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
            (
                select 
                    json_agg(
                        json_build_object(
                            'Path', t.path,
                            'Name', t.name
                        )
                    )
                    FROM (
                        SELECT
                            case 
                                when tn2.url_path is null then '/node/' || tn2.url_id
                                else '/' || tn2.url_path
                            end path,
                            t.name
                        FROM node_term nt
                        JOIN tenant_node tn1 on tn1.node_id = nt.node_id AND tn1.tenant_id = @tenant_id AND tn1.publication_status_id = 1 
                        JOIN term t on t.id = nt.term_id
                        JOIN tenant_node tn2 on tn2.node_id = t.nameable_id AND tn2.tenant_id = @tenant_id AND tn2.publication_status_id = 1 
                        WHERE nt.node_id = n.id
                    ) t
             ) "Tags"
             FROM node n
        	 join article a on a.id = n.id
             join simple_text_node stn on stn.id = n.id
        	 join publisher p on p.id = n.publisher_id
             JOIN tenant_node tn on tn.node_id = n.id AND tn.tenant_id = @tenant_id AND tn.publication_status_id = 1 
        	 ORDER BY n.changed_date_time DESC
        	 LIMIT @length OFFSET @start_index
        	)
        """;


    const string FETCH_ARTICLES_FILTERED = """
        fetch_articles as (	
            select
            tna.url_id "Id",
            n.title "Title",
            stn.teaser "Text",
            json_build_object(
                'Id', p.id,
                'Name', p.name,
                'CreatedDateTime', n.created_date_time,
                'ChangedDateTime', n.changed_date_time 
            ) "Authoring",
            (
                select 
                    json_agg(
                        json_build_object(
                            'Path', t.path,
                            'Name', t.name
                        )
                    )
                    FROM (
                        SELECT
                            case 
                                when tn2.url_path is null then '/node/' || tn2.url_id
                                else '/' || tn2.url_path
                            end path,
                            t.name
                        FROM node_term nt
                        JOIN tenant_node tn1 on tn1.node_id = nt.node_id AND tn1.tenant_id = @tenant_id AND tn1.publication_status_id = 1 
                        JOIN term t on t.id = nt.term_id
                        JOIN tenant_node tn2 on tn2.node_id = t.nameable_id AND tn2.tenant_id = @tenant_id AND tn2.publication_status_id = 1 
                        WHERE nt.node_id = n.id
                    ) t
             ) "Tags"
            FROM node n
            join publisher p on p.id = n.publisher_id
            join simple_text_node stn on stn.id = n.id
            JOIN tenant_node tna on tna.node_id = n.id AND tna.tenant_id = @tenant_id AND tna.publication_status_id = 1
            join article a on a.id = n.id
            join tenant te on te.id = @tenant_id
            join tenant_node tnt on tnt.url_id in ({0}) AND tnt.tenant_id = @tenant_id AND tnt.publication_status_id = 1 
            JOIN nameable nm on nm.id = tnt.node_id
            JOIN term t on t.nameable_id = tnt.node_id and t.vocabulary_id = te.vocabulary_id_tagging
            left join node_term nt on nt.term_id = t.id and nt.node_id = n.id
        	GROUP BY
            tna.url_id,
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

    const string COUNT_ARTICLES_FILTERED = """
        count_articles as(
            select
                count(*) count
            from(
                select
        		    n.id
                FROM node n
                JOIN tenant_node tn on tn.node_id = n.id AND tn.tenant_id = 1 AND tn.publication_status_id = 1 
                join article a on a.id = n.id
                JOIN tenant_node tn2 on tn2.url_id in ({0}) AND tn2.tenant_id =1 AND tn2.publication_status_id = 1 
                join term t on t.nameable_id = tn2.node_id
                left join node_term nt on nt.node_id = n.id and nt.term_id = t.id
                GROUP BY n.id
                HAVING COUNT(n.id) = COUNT(nt.node_id)
            ) c
        )
        """;

    const string COUNT_ARTICLES_UNFILTERED = """
        count_articles as(
            select
                count(n.id)
            FROM node n
            JOIN tenant_node tn on tn.node_id = n.id AND tn.tenant_id = 1 AND tn.publication_status_id = 1 
            join article a on a.id = n.id
        )
        """;

    const string FETCHS_ARTICLES_DOCUMENTS = """
        fetch_articles_document as(
        	select json_agg(to_jsonb(x)) agg
        	from fetch_articles x
        )
        """;

    public async Task<Articles> GetArticles(List<int> selectedTerms, int startIndex, int length, int tenantId)
    {
        var termsList = string.Join(',', selectedTerms.Select(x => x.ToString()));
        try
        {
            await _connection.OpenAsync();
            var sql = $"""
            WITH
            {string.Format(FETCH_TERMS_FILTERED, termsList)},
            {string.Format(FETCH_ARTICLES_FILTERED, termsList)},
            {string.Format(COUNT_ARTICLES_FILTERED, termsList)},
            {FETCHS_ARTICLES_DOCUMENTS}
            select to_jsonb(ta)
            from(
            select 
            	(select * from fetch_terms)  "TermNames",
            	(select agg from fetch_articles_document) "ArticleListEntries",
                (select count from count_articles) "NumberOfEntries"
            	) ta
            """;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("tenant_id", NpgsqlDbType.Integer);
            readCommand.Parameters.Add("length", NpgsqlDbType.Integer);
            readCommand.Parameters.Add("start_index", NpgsqlDbType.Integer);
            await readCommand.PrepareAsync();
            readCommand.Parameters["tenant_id"].Value = tenantId;
            readCommand.Parameters["length"].Value = length;
            readCommand.Parameters["start_index"].Value = startIndex;
            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            var articles = reader.GetFieldValue<Articles>(0);
            return articles;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<Articles> GetArticles(int startIndex, int length, int tenantId)
    {
        _connection.Open();
        var sql = $"""
            WITH
            {FETCH_TERMS},
            {FETCH_ARTICLES},
            {FETCHS_ARTICLES_DOCUMENTS},
            {COUNT_ARTICLES_UNFILTERED}
            select to_jsonb(ta)
            from(
            select 
            	(select * from fetch_terms)  "TermNames",
            	(select agg from fetch_articles_document) "ArticleListEntries",
                (select count from count_articles) "NumberOfEntries"
            	) ta
            """;

        using var readCommand = _connection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;
        readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        readCommand.Parameters.Add("length", NpgsqlDbType.Integer);
        readCommand.Parameters.Add("start_index", NpgsqlDbType.Integer);
        await readCommand.PrepareAsync();
        readCommand.Parameters["tenant_id"].Value = tenantId;
        readCommand.Parameters["length"].Value = length;
        readCommand.Parameters["start_index"].Value = startIndex;
        await using var reader = await readCommand.ExecuteReaderAsync();
        await reader.ReadAsync();
        var articles = reader.GetFieldValue<Articles>(0);
        _connection.Close();
        return articles;
    }
}
