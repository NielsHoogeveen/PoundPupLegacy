using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class OrganizationMigratorPPL: PPLMigrator
{
    public OrganizationMigratorPPL(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "organizations (ppl)";

    private async IAsyncEnumerable<Organization> GetOrganizations()
    {
        yield return new Organization
        {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "Colorado Adoption Center",
            OwnerId = Constants.OWNER_PARTIES,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.COLORADO_ADOPTION_CENTER
                    }
                },
            NodeTypeId = 23,
            WebsiteURL = null,
            EmailAddress = null,
            Established = null,
            Terminated = null,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>(),
            OrganizationTypes = new List<OrganizationOrganizationType>
            {
                new OrganizationOrganizationType
                {
                    OrganizationId = null,
                    OrganizationTypeId = await _nodeIdReader.ReadAsync(Constants.PPL, 12625)
                }
            }
        };
        yield return new Organization
        {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "Government of Italy",
            OwnerId = Constants.OWNER_PARTIES,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = 17036
                    }
                },
            NodeTypeId = 23,
            WebsiteURL = null,
            EmailAddress = null,
            Established = null,
            Terminated = null,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>(),
            OrganizationTypes = new List<OrganizationOrganizationType>
            {
                new OrganizationOrganizationType
                {
                    OrganizationId = null,
                    OrganizationTypeId = await _nodeIdReader.ReadAsync(Constants.PPL, 12630)
                }
            }
        };
    }

    protected override async Task MigrateImpl()
    {
        await OrganizationCreator.CreateAsync(GetOrganizations(), _postgresConnection);
        await OrganizationCreator.CreateAsync(ReadOrganizations(), _postgresConnection);
    }
    private async IAsyncEnumerable<Organization> ReadOrganizations()
    {

        var sql = $"""
            SELECT
                n.nid id,
                n.uid access_role_id,
                n.title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                23 node_type_id,
                o.field_website_2_url website_url,
                FROM_UNIXTIME(o.field_start_date_0_value) established, 
                FROM_UNIXTIME(UNIX_TIMESTAMP(o.field_end_date_value)) `terminated`,
                o.field_email_address_email email_address,
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
                	group_concat(nots.nid SEPARATOR ',') organization_types
            FROM node n 
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            LEFT JOIN category_node cna ON cna.nid = n.nid AND cna.cid = 38518
            LEFT JOIN category_node cnb ON cnb.nid = n.nid AND cnb.cid = 38308
            JOIN content_type_adopt_orgs o ON o.nid = n.nid AND o.vid = n.vid
            JOIN category_node cnot ON cnot.nid = n.nid
            JOIN category `cot` ON `cot`.cid = cnot.cid AND `cot`.cnid = 12622
            JOIN node nots ON nots.nid = `cot`.cid
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
                	WHERE n.`type` = 'adopt_orgs'
            AND n.nid NOT IN (11108, 7760, 12700, 30638)
            AND cna.nid IS NULL
            AND cnb.nid IS NULL
            GROUP BY
                n.nid,
                n.uid,
                n.title,
                n.`status`,
                n.created, 
                n.`changed`,
                o.field_website_2_url,
                o.field_start_date_0_value, 
                o.field_end_date_value,
                o.field_email_address_email,
                o.field_description_3_value,
                c.title,
                c2.title,
                	c.topic_parent_names ,
                	c2.topic_parent_names,
                ua.dst
            """;
        using var readCommand = _mysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var vocabularyNames = new List<VocabularyName>();


            var typeIds = reader
                            .GetString("organization_types")
                            .Split(',')
                            .Where(x=> !string.IsNullOrEmpty(x))
                            .Select(x => int.Parse(x));
            var organizationOrganizationTypes = new List<OrganizationOrganizationType>();
            foreach(var typeId in typeIds)
            {
                var organizationTypeId = await _nodeIdReader.ReadAsync(Constants.PPL, typeId);
                organizationOrganizationTypes.Add(new OrganizationOrganizationType { OrganizationId = null, OrganizationTypeId = organizationTypeId });
            }

            var id = reader.GetInt32("id");

            if (!reader.IsDBNull("topic_name"))
            {
                var topicName = reader.GetString("topic_name");
                var topicParentNames = reader.IsDBNull("topic_parent_names") ? 
                    new List<string>() : reader.GetString("topic_parent_names")
                    .Split(',')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => x.Replace("Colorado", "Colorado (state of the USA)"))
                    .Select(x => x.Replace("New York", "New York (state of the USA)"))
                    .Select(x => x.Replace("Illinois", "Illinois (state of the USA)"))
                    .Select(x => x.Replace("Texas", "Texas (state of the USA)"))
                    .Select(x => x.Replace("Utah", "Texas (state of the USA)"))
                    .Select(x => x.Replace("Arizona", "Arizona (state of the USA)"))
                    .Select(x => x.Replace("Connecticut", "Connecticut (state of the USA)"))
                    .Select(x => x.Replace("District of Columbia", "District of Columbia (state of the USA)"))
                    .Select(x => x.Replace("Florida", "Florida (state of the USA)"))
                    .Select(x => x.Replace("Georgia (state)", "Georgia (state of the USA)"))
                    .Select(x => x.Replace("Kansas", "Kansas (state of the USA)"))
                    .Select(x => x.Replace("Kentucky", "Kentucky (state of the USA)"))
                    .Select(x => x.Replace("Maine", "Maine (state of the USA)"))
                    .Select(x => x.Replace("Michigan", "Michigan (state of the USA)"))
                    .Select(x => x.Replace("Mississippi", "Mississippi (state of the USA)"))
                    .Select(x => x.Replace("Nebraska", "Nebraska (state of the USA)"))
                    .Select(x => x.Replace("New Jersey", "New Jersey (state of the USA)"))
                    .Select(x => x.Replace("Oklahoma", "Oklahoma (state of the USA)"))
                    .Select(x => x.Replace("Oregon", "Oregon (state of the USA)"))
                    .Select(x => x.Replace("South Carolina", "South Carolina (state of the USA)"))
                    .Select(x => x.Replace("Tennessee", "Tennessee (state of the USA)"))
                    .Select(x => x.Replace("Washington", "Washington (state of the USA)"))
                    .Select(x => x.Replace("Wisconsin", "Wisconsin (state of the USA)"))
                    .Select(x => x.Replace("Missouri", "Missouri (state of the USA)"))
                    .ToList();

                vocabularyNames.Add(new VocabularyName
                {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = topicName,
                    ParentNames = topicParentNames,
                });
            }


            yield return new Organization
            {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = reader.GetString("title"),
                OwnerId = Constants.OWNER_PARTIES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = reader.GetInt16("node_type_id"),
                WebsiteURL = reader.IsDBNull("website_url") ? null : reader.GetString("website_url"),
                EmailAddress = reader.IsDBNull("email_address") ? null : reader.GetString("email_address"),
                Description = reader.IsDBNull("description") ? "" : reader.GetString("description"),
                Established = reader.IsDBNull("established") ? null : reader.GetDateTime("established"),
                Terminated = reader.IsDBNull("terminated") ? null : reader.GetDateTime("terminated"),
                FileIdTileImage = null,
                VocabularyNames = vocabularyNames,
                OrganizationTypes = organizationOrganizationTypes,
            };

        }
        await reader.CloseAsync();
    }


}
