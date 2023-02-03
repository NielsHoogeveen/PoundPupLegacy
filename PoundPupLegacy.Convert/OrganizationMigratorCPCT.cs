using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class OrganizationMigratorCPCT: CPCTMigrator
{
    public OrganizationMigratorCPCT(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "organizations (cpct)";


    protected override async Task MigrateImpl()
    {
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
                	group_concat(nots.nid SEPARATOR ',') organization_types,
                	group_concat(nots.title SEPARATOR ',') organization_type_names
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
            AND n.nid  > 33163
            AND n.nid not in (
                34880, 
                35725, 
                33644, 
                36680, 
                34973, 
                45964, 
                43829, 
                35124, 
                34126, 
                34082, 
                34015, 
                39431, 
                49152, 
                35138, 
                35146, 
                33255, 
                38588, 
                44216, 
                48210, 
                48330, 
                47699, 
                49224, 
                35233,
                49624, 
                33454,
                50995)
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

        /*
         * 34880 => 35760
         * 35725 => 37560
         * 33644 => 38279
         * 36680 => 39755
         * 34973 => 40525
         * 45964 => 41694
         * 35124 => 45974
         * 34126 => 48192
         * 34082 => 52502
         * 34015 => 53756
         * 49152 => 60032
         * 35138 => 73657
         * 35146 => 73661
         * 33255 => 33987
         * 44216 => 29148
         * 49224 => 35567
         * 35233 => 44675
         * 33454 => 35190
         * 48210 => 34899 cpct
         * 48330 => 48545 cpct
         * 47699 => 48846 cpct
         */
        using var readCommand = MysqlConnection.CreateCommand();
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
                            .Where(x => !string.IsNullOrEmpty(x))
                            .Select(x => int.Parse(x));
            var organizationOrganizationTypes = new List<OrganizationOrganizationType>();
            foreach (var typeId in typeIds)
            {
                var organizationTypeId = await _nodeIdReader.ReadAsync(Constants.PPL, typeId);
                organizationOrganizationTypes.Add(new OrganizationOrganizationType { OrganizationId = null, OrganizationTypeId = organizationTypeId });
            }

            var id = reader.GetInt32("id");


            var tenantNodes = new List<TenantNode>
            {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
            };
            if (!organizationOrganizationTypes.Select(x => x.OrganizationTypeId).Contains(12634))
            {
                tenantNodes.Add(new TenantNode
                {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = null
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
                TenantNodes = tenantNodes,
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
