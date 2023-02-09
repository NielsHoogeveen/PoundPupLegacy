using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class BillMigrator : PPLMigrator
{
    public BillMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "bills";

    protected override async Task MigrateImpl()
    {
        await HouseBillCreator.CreateAsync(ReadHouseBills(), _postgresConnection);
        await SenateBillCreator.CreateAsync(ReadSenateBills(), _postgresConnection);

    }

    private async IAsyncEnumerable<HouseBill> ReadHouseBills()
    {

        var sql = $"""
                 select
                 *
                 FROM(
                 SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    o.field_description_3_value description,
                  	case 
                  		when c.title IS NOT NULL then c.title
                  		ELSE c2.title
                  	END topic_name,
                  	case 
                  		when c.topic_parent_names IS NOT NULL then c.topic_parent_names
                  		ELSE c2.topic_parent_names
                  	END topic_parent_names,
                    ua.dst url_path,
                   STR_TO_DATE(i.field_start_date_value ,'%Y-%m-%d') introduction_date
                 FROM node n
                 LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                 JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                 JOIN content_type_adopt_orgs o ON o.nid = n.nid AND o.vid = n.vid
                 JOIN category_node cn ON cn.nid = n.nid AND cn.cid = 38308
                 LEFT JOIN (
                 	select
                 		n.nid,
                 		n.title,
                 		cc.field_tile_image_title,
                 		cc.field_related_page_nid,
                 		GROUP_CONCAT(p.title, ',') topic_parent_names
                 	FROM node n
                 	JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                 	LEFT JOIN (
                 		SELECT
                 			n.nid, 
                 			n.title,
                 			ch.cid
                 		FROM node n
                 		JOIN category_hierarchy ch ON ch.parent = n.nid
                 		WHERE n.`type` = 'category_cat'
                 	) p ON p.cid = n.nid
                 	GROUP BY 
                 		n.nid,
                 		n.title,
                 		cc.field_tile_image_title,
                 		cc.field_related_page_nid
                 	) c ON c.field_related_page_nid = n.nid
                 	LEFT JOIN (
                 	select
                 		n.nid,
                 		n.title,
                 		GROUP_CONCAT(p.title, ',') topic_parent_names
                 	FROM node n
                 	JOIN category c ON c.cid = n.nid AND c.cnid = 4126
                 	LEFT JOIN (
                 		SELECT
                 			n.nid, 
                 			n.title,
                 			ch.cid
                 		FROM node n
                 		JOIN category_hierarchy ch ON ch.parent = n.nid
                 		WHERE n.`type` = 'category_cat'
                 	) p ON p.cid = n.nid
                 	GROUP BY 
                 		n.nid,
                 		n.title
                 ) c2 ON c2.title = n.title
                 LEFT JOIN (
                 	SELECT
                 	p.field_industrial_organisation_nid,
                 	p.field_industrialist_nid,
                 	n2.title,
                 	p.field_start_date_value
                 	FROM node n
                 	JOIN content_type_adopt_positions p ON p.nid = n.nid AND p.vid = n.vid
                	JOIN category_node cn ON cn.nid = n.nid AND cn.cid = 38509  
                	JOIN node n2 ON n2.nid = p.field_industrialist_nid
                    WHERE n.nid <> 59369
                 ) i ON i.field_industrial_organisation_nid = n.nid
                 WHERE n.`type` = 'adopt_orgs' AND n.uid <> 0 AND n.nid NOT IN (73678, 64018, 59410, 64614, 54123, 64123, 64297, 64324, 64151)
                 ) X
                 WHERE SUBSTRING(title, 1, 2) = 'H.'
                
                """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {

            var vocabularyNames = new List<VocabularyName>();

            var id = reader.GetInt32("id");

            if (!reader.IsDBNull("topic_name"))
            {
                var topicName = reader.GetString("topic_name");
                var topicParentNames = reader.IsDBNull("topic_parent_names") ?
                    new List<string>() : reader.GetString("topic_parent_names")
                    .Split(',')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                vocabularyNames.Add(new VocabularyName
                {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = topicName,
                    ParentNames = topicParentNames,
                });
            }

            yield return new HouseBill
            {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                OwnerId = Constants.PPL,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("status"),
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = 36,
                Description = reader.GetString("description"),
                VocabularyNames = vocabularyNames,
                FileIdTileImage = null,
                IntroductionDate = reader.IsDBNull("introduction_date") ? null : reader.GetDateTime("introduction_date"),
            };
        }
        await reader.CloseAsync();
    }
    private async IAsyncEnumerable<SenateBill> ReadSenateBills()
    {

        var sql = $"""
                 select
                 *
                 FROM(
                 SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    o.field_description_3_value description,
                  	case 
                  		when c.title IS NOT NULL then c.title
                  		ELSE c2.title
                  	END topic_name,
                  	case 
                  		when c.topic_parent_names IS NOT NULL then c.topic_parent_names
                  		ELSE c2.topic_parent_names
                  	END topic_parent_names,
                    ua.dst url_path,
                   STR_TO_DATE(i.field_start_date_value ,'%Y-%m-%d') introduction_date
                 FROM node n
                 LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                 JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                 JOIN content_type_adopt_orgs o ON o.nid = n.nid AND o.vid = n.vid
                 JOIN category_node cn ON cn.nid = n.nid AND cn.cid = 38308
                 LEFT JOIN (
                 	select
                 		n.nid,
                 		n.title,
                 		cc.field_tile_image_title,
                 		cc.field_related_page_nid,
                 		GROUP_CONCAT(p.title, ',') topic_parent_names
                 	FROM node n
                 	JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                 	LEFT JOIN (
                 		SELECT
                 			n.nid, 
                 			n.title,
                 			ch.cid
                 		FROM node n
                 		JOIN category_hierarchy ch ON ch.parent = n.nid
                 		WHERE n.`type` = 'category_cat'
                 	) p ON p.cid = n.nid
                 	GROUP BY 
                 		n.nid,
                 		n.title,
                 		cc.field_tile_image_title,
                 		cc.field_related_page_nid
                 	) c ON c.field_related_page_nid = n.nid
                 	LEFT JOIN (
                 	select
                 		n.nid,
                 		n.title,
                 		GROUP_CONCAT(p.title, ',') topic_parent_names
                 	FROM node n
                 	JOIN category c ON c.cid = n.nid AND c.cnid = 4126
                 	LEFT JOIN (
                 		SELECT
                 			n.nid, 
                 			n.title,
                 			ch.cid
                 		FROM node n
                 		JOIN category_hierarchy ch ON ch.parent = n.nid
                 		WHERE n.`type` = 'category_cat'
                 	) p ON p.cid = n.nid
                 	GROUP BY 
                 		n.nid,
                 		n.title
                 ) c2 ON c2.title = n.title
                 LEFT JOIN (
                 	SELECT
                 	p.field_industrial_organisation_nid,
                 	p.field_industrialist_nid,
                 	n2.title,
                 	p.field_start_date_value
                 	FROM node n
                 	JOIN content_type_adopt_positions p ON p.nid = n.nid AND p.vid = n.vid
                	JOIN category_node cn ON cn.nid = n.nid AND cn.cid = 38509  
                	JOIN node n2 ON n2.nid = p.field_industrialist_nid
                    WHERE n.nid not in (64273, 73684, 73685, 73686)
                 ) i ON i.field_industrial_organisation_nid = n.nid
                 WHERE n.`type` = 'adopt_orgs' AND n.uid <> 0 AND n.nid NOT IN (73678, 64018, 59410, 64614, 54123, 64123, 64297, 64324, 64151)
                 ) X
                 WHERE SUBSTRING(title, 1, 2) = 'S.'
                
                """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {

            var vocabularyNames = new List<VocabularyName>();

            var id = reader.GetInt32("id");

            if (!reader.IsDBNull("topic_name"))
            {
                var topicName = reader.GetString("topic_name");
                var topicParentNames = reader.IsDBNull("topic_parent_names") ?
                    new List<string>() : reader.GetString("topic_parent_names")
                    .Split(',')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                vocabularyNames.Add(new VocabularyName
                {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = topicName,
                    ParentNames = topicParentNames,
                });
            }

            yield return new SenateBill
            {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                OwnerId = Constants.PPL,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("status"),
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = 36,
                Description = reader.GetString("description"),
                VocabularyNames = vocabularyNames,
                FileIdTileImage = null,
                IntroductionDate = reader.IsDBNull("introduction_date") ? null : reader.GetDateTime("introduction_date"),
            };
        }
        await reader.CloseAsync();
    }

}
