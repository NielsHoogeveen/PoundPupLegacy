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
        			select 
        				t.id "Id",
        				t.name "Name",
                        false "Selected"
        			from article a
        			join node n on n.id = a.id 
        			join node_term nt on nt.node_id = n.id
        			join term t on t.id = nt.term_id
        			where n.node_status_id = 1
        			group by 
        				t.id,
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
        			select 
        				t.id "Id",
        				t.name "Name",
                        case
                            when t.id in ({0}) then true
                            else false
                        end "Selected"
        			from article a
        			join node n on n.id = a.id 
        			join node_term nt on nt.node_id = n.id
        			join term t on t.id = nt.term_id
        			where n.node_status_id = 1
                    AND n.id in (
                        SELECT
                            n.id
                        FROM node n
                        join article a on a.id = n.id
                        join access_role ar on ar.id = n.access_role_id
        	            join term t on t.id in ({0})
        	            left join node_term nt on nt.node_id = n.id and nt.term_id = t.id
        	            GROUP BY
        	                n.id
                        HAVING COUNT(n.id) = COUNT(nt.node_id)
                    )
        			group by 
        				t.id,
        				t.name
        			order by count(a.id) desc, t.name
        			LIMIT 15
        		) name
        	 )
        """;

    const string FETCH_ARTICLES = """
        fetch_articles as (	
        	 select
        	 n.id "Id",
        	 n.title "Title",
             stn.text "Text",
        	 json_build_object(
        	    'Id', ar.id,
        		'Name', ar.name,
        		'CreatedDateTime', n.created_date_time,
        		'ChangedDateTime', n.changed_date_time 
        	 ) "Authoring",
             (select json_agg(json_build_object(
                    'Id', t.id,
                    'Name', t.name
                ))
                FROM node_term nt
                JOIN term t on t.id = nt.term_id
                WHERE nt.node_id = n.id
             ) "Tags"
        	 FROM node n
        	 join article a on a.id = n.id
             join simple_text_node stn on stn.id = n.id
        	 join access_role ar on ar.id = n.access_role_id
             WHERE n.node_status_id = 1
        	 ORDER BY n.changed_date_time DESC
        	 LIMIT 10
        	)
        """;
    const string FETCH_ARTICLES_FILTERED = """
        fetch_articles as (	
            select
            n.id "Id",
            n.title "Title",
            stn.text "Text",
            json_build_object(
                'Id', ar.id,
                'Name', ar.name,
                'CreatedDateTime', n.created_date_time,
                'ChangedDateTime', n.changed_date_time 
            ) "Authoring",
            (select json_agg(json_build_object(
                    'Id', t.id,
                    'Name', t.name
                ))
                FROM node_term nt
                JOIN term t on t.id = nt.term_id
                WHERE nt.node_id = n.id
             ) "Tags"
            FROM node n
            join article a on a.id = n.id
            join simple_text_node stn on stn.id = n.id
            join access_role ar on ar.id = n.access_role_id
        	join term t on t.id in ({0})
        	left join node_term nt on nt.node_id = n.id and nt.term_id = t.id
            WHERE n.node_status_id = 1
        	GROUP BY
        	n.id,
            n.title,
            stn.text,
            ar.id,
            ar.name,
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
        await readCommand.PrepareAsync();
        await using var reader = await readCommand.ExecuteReaderAsync();
        await reader.ReadAsync();
        var articles = reader.GetFieldValue<Articles>(0);
        _connection.Close();
        var teasers = articles.ArticleListEntries.Select(e => new ArticleListEntry
        {
            Id = e.Id,
            Authoring = e.Authoring,
            Title = e.Title,
            Text = _teaserService.MakeTeaser(e.Text),
            Tags = e.Tags,
        });

        return new Articles
        {
            TermNames = articles.TermNames,
            ArticleListEntries = teasers.ToList(),
        };
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
        await readCommand.PrepareAsync();
        await using var reader = await readCommand.ExecuteReaderAsync();
        await reader.ReadAsync();
        var articles = reader.GetFieldValue<Articles>(0);
        _connection.Close();
        var teasers = articles.ArticleListEntries.Select(e => new ArticleListEntry
        {
            Id = e.Id,
            Authoring = e.Authoring,
            Title = e.Title,
            Text = _teaserService.MakeTeaser(e.Text),
            Tags = e.Tags,
        });

        return new Articles
        {
            TermNames = articles.TermNames,
            ArticleListEntries = teasers.ToList(),
        };
    }
}
