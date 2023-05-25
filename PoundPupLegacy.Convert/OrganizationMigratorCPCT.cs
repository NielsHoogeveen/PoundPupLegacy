﻿using PoundPupLegacy.CreateModel.Creators;

namespace PoundPupLegacy.Convert;

internal sealed class OrganizationMigratorCPCT(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    ISingleItemDatabaseReaderFactory<TenantNodeReaderByUrlIdRequest, NewTenantNodeForNewNode> tenantNodeReaderByUrlIdFactory,
    ISingleItemDatabaseReaderFactory<TermReaderByNameableIdRequest, CreateModel.Term> termReaderByNameableIdFactory,
    IEntityCreatorFactory<EventuallyIdentifiableOrganization> organizationCreatorFactory
) : MigratorCPCT(
    databaseConnections, 
    nodeIdReaderFactory, 
    tenantNodeReaderByUrlIdFactory
)
{
    protected override string Name => "organizations (cpct)";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termReaderByNameableId = await termReaderByNameableIdFactory.CreateAsync(_postgresConnection);
        await using var organizationCreator = await organizationCreatorFactory.CreateAsync(_postgresConnection);
        await organizationCreator.CreateAsync(ReadOrganizations(nodeIdReader, termReaderByNameableId));
    }

    private async IAsyncEnumerable<NewBasicOrganization> ReadOrganizations(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        ISingleItemDatabaseReader<TermReaderByNameableIdRequest, CreateModel.Term> termReaderByNameableId
    )
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
                47216,
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
                50995,
                36502)
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
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();
        var miscellaneous = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = 12634
        });

        while (await reader.ReadAsync()) {
            var name = reader.GetString("title");
            var typeIds = reader
                .GetString("organization_types")
                .Split(',')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => int.Parse(x));
            var organizationOrganizationTypeIds = new List<int>();
            foreach (var typeId in typeIds) {
                var organizationTypeId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = typeId
                });
                organizationOrganizationTypeIds.Add(organizationTypeId);
            }
            async IAsyncEnumerable<string> GetTermNamesForOrganizationsTypes(IEnumerable<int> organizationTypeIds)
            {
                foreach (var organizationTypeId in organizationTypeIds) {
                    var res = await termReaderByNameableId.ReadAsync(new TermReaderByNameableIdRequest {
                        NameableId = organizationTypeId,
                        OwnerId = Constants.OWNER_SYSTEM,
                        VocabularyName = Constants.VOCABULARY_TOPICS
                    });
                    yield return res!.Name;
                }
            }
            var vocabularyNames = new List<VocabularyName> {
                new VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = name,
                    ParentNames = await GetTermNamesForOrganizationsTypes(organizationOrganizationTypeIds).ToListAsync(),
                }
            };

            var id = reader.GetInt32("id");


            var tenantNodes = new List<NewTenantNodeForNewNode>
            {
                    new NewTenantNodeForNewNode
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

            var toSkipForPPL = new List<int> { 34447, 42413, 46479, 48178, 39305, 45402, 46671, 33634, 48051 };

            if (!organizationOrganizationTypeIds.Contains(miscellaneous) && !toSkipForPPL.Contains(id)) {
                tenantNodes.Add(new NewTenantNodeForNewNode {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = null
                });
            }

            yield return new NewBasicOrganization {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = reader.GetString("title"),
                OwnerId = Constants.OWNER_PARTIES,
                AuthoringStatusId = 1,
                TenantNodes = tenantNodes,
                NodeTypeId = reader.GetInt16("node_type_id"),
                WebsiteUrl = reader.IsDBNull("website_url") ? null : reader.GetString("website_url"),
                EmailAddress = reader.IsDBNull("email_address") ? null : reader.GetString("email_address"),
                Description = reader.IsDBNull("description") ? "" : reader.GetString("description"),
                Established = reader.IsDBNull("established") ? null : (new DateTimeRange(reader.GetDateTime("established").Date, reader.GetDateTime("established").Date.AddDays(1).AddMilliseconds(-1))).ToFuzzyDate(),
                Terminated = reader.IsDBNull("terminated") ? null : (new DateTimeRange(reader.GetDateTime("terminated").Date, reader.GetDateTime("terminated").Date.AddDays(1).AddMilliseconds(-1))).ToFuzzyDate(),
                FileIdTileImage = null,
                VocabularyNames = vocabularyNames,
                OrganizationTypeIds = organizationOrganizationTypeIds,
                NodeTermIds = new List<int>(),
                NewLocations = new List<EventuallyIdentifiableLocation>(),
                PartyPoliticalEntityRelations = new List<EventuallyIdentifiablePartyPoliticalEntityRelationForNewParty>(),
                PersonOrganizationRelations = new List<EventuallyIdentifiablePersonOrganizationRelationForNewOrganization>(),
                InterOrganizationalRelationsToAddFrom = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationFrom>(),
                InterOrganizationalRelationsToAddTo = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo>(),
            };

        }
        await reader.CloseAsync();
    }

}
