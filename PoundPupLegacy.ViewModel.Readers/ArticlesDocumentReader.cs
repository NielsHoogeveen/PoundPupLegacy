using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.ViewModel.Readers;


public class ArticlesDocumentReader : DatabaseReader, ISingleItemDatabaseReader<ArticlesDocumentReader, ArticlesDocumentReader.ArticlesDocumentRequest, Articles>
{
    public record ArticlesDocumentRequest
    {
        public required int TenantId { get; init; }
        public required List<int> SelectedTerms { get; init; }
        public required int StartIndex { get; init; }
        public required int Length { get; init; }
    }
    private ArticlesDocumentReader(NpgsqlCommand command) : base(command)
    {
    }

    public async Task<Articles> ReadAsync(ArticlesDocumentRequest request)
    {
            _command.Parameters["tenant_id"].Value = request.TenantId;
            _command.Parameters["length"].Value = request.Length;
            _command.Parameters["start_index"].Value = request.StartIndex;
            if (request.SelectedTerms.Any()) {
                _command.Parameters["terms"].Value = request.SelectedTerms.ToArray();
            }
            else {
                _command.Parameters["terms"].Value = DBNull.Value;
            }
            await using var reader = await _command.ExecuteReaderAsync();
            await reader.ReadAsync();
            var articles = reader.GetFieldValue<Articles>(0);
            return articles;
    }


    public static async Task<ArticlesDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        command.Parameters.Add("tenant_id", NpgsqlDbType.Integer);
        command.Parameters.Add("length", NpgsqlDbType.Integer);
        command.Parameters.Add("start_index", NpgsqlDbType.Integer);
        command.Parameters.Add("terms", NpgsqlDbType.Array | NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new ArticlesDocumentReader(command);

    }
    const string SQL = $"""
            WITH
            {FETCH_TERMS_UNFILTERED},
            {FETCH_TERMS_FILTERED},
            {FETCH_ARTICLES_UNFILTERED},
            {FETCH_ARTICLES_FILTERED},
            {COUNT_ARTICLES_UNFILTERED},
            {COUNT_ARTICLES_FILTERED},
            {FETCHS_ARTICLES_DOCUMENTS_FILTERED},
            {FETCHS_ARTICLES_DOCUMENTS_UNFILTERED}
            select to_jsonb(ta)
            from(
            select 
                case 
                    when @terms is null then (select * from fetch_terms_unfiltered)  
                    else (select * from fetch_terms_filtered)  
                end "TermNames",
                case 
                    when @terms is null then (select agg from fetch_articles_document_unfiltered) 
                    else (select agg from fetch_articles_document_filtered) 
                end "ArticleListEntries",
                case
                    when @terms is null then (select count from count_articles_unfiltered) 
                    else (select count from count_articles_filtered) 
                end "NumberOfEntries"
            	) ta
            """;

    const string FETCH_TERMS_UNFILTERED = """
         fetch_terms_unfiltered as (
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
         fetch_terms_filtered as (
         select 
        	json_agg(name)
        		FROM(
        			SELECT 
        				tn.url_id "Id",
        				t.name "Name",
                        CASE
                            WHEN tn.url_id = any(@terms) THEN true
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
                        join tenant_node tnt on tnt.url_id = any(@terms) AND tnt.tenant_id = @tenant_id AND tnt.publication_status_id = 1 
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

    const string FETCH_ARTICLES_UNFILTERED = """
        fetch_articles_unfiltered as (	
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
        fetch_articles_filtered as (	
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
            join tenant_node tnt on tnt.url_id = any(@terms) AND tnt.tenant_id = @tenant_id AND tnt.publication_status_id = 1 
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
        count_articles_filtered as(
            select
                count(*) count
            from(
                select
        		    n.id
                FROM node n
                JOIN tenant_node tn on tn.node_id = n.id AND tn.tenant_id = 1 AND tn.publication_status_id = 1 
                join article a on a.id = n.id
                JOIN tenant_node tn2 on tn2.url_id = any(@terms) AND tn2.tenant_id =1 AND tn2.publication_status_id = 1 
                join term t on t.nameable_id = tn2.node_id
                left join node_term nt on nt.node_id = n.id and nt.term_id = t.id
                GROUP BY n.id
                HAVING COUNT(n.id) = COUNT(nt.node_id)
            ) c
        )
        """;

    const string COUNT_ARTICLES_UNFILTERED = """
        count_articles_unfiltered as(
            select
                count(n.id)
            FROM node n
            JOIN tenant_node tn on tn.node_id = n.id AND tn.tenant_id = 1 AND tn.publication_status_id = 1 
            join article a on a.id = n.id
        )
        """;

    const string FETCHS_ARTICLES_DOCUMENTS_UNFILTERED = """
        fetch_articles_document_unfiltered as(
        	select json_agg(to_jsonb(x)) agg
        	from fetch_articles_unfiltered x
        )
        """;
    const string FETCHS_ARTICLES_DOCUMENTS_FILTERED = """
        fetch_articles_document_filtered as(
        	select json_agg(to_jsonb(x)) agg
        	from fetch_articles_filtered x
        )
        """;

}
