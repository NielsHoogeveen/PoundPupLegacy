using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.Convert;

internal sealed class OrganizationTypeMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileIdFactory,
    IEntityCreatorFactory<EventuallyIdentifiableOrganizationType> organizationTypeCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "organization types";

    protected override async Task MigrateImpl()
    {
        await using var fileIdReaderByTenantFileId = await fileIdReaderByTenantFileIdFactory.CreateAsync(_postgresConnection);
        await using var organizationTypeCreator = await organizationTypeCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await organizationTypeCreator.CreateAsync(ReadOrganizationTypes(nodeIdReader, termIdReader, fileIdReaderByTenantFileId));
    }
    private async IAsyncEnumerable<NewOrganizationType> ReadOrganizationTypes(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        IMandatorySingleItemDatabaseReader<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileId
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
                case 
                    when n2.body is NULL then ''
                    ELSE n2.body
                END description,
                case 
                    when n2.field_tile_image_fid = 0 then null
                    ELSE n2.field_tile_image_fid
                END file_id_tile_image,
                case
                    when n2.title IS NOT NULL then n2.title
                    ELSE LOWER(n.title)
                END topic_name,
                case
                    when n.nid = 12626 then 'adoption organizations,'
                    when n.nid = 12624 then 'adoption lobby,'
                    when n2.parent_topics IS NOT NULL then n2.parent_topics
                    ELSE 'organizations,'
                END topic_parent_names,
                case
                when n.nid IN (14670,28962) then true
                ELSE false
                END has_concrete_subtype,
                ua.dst url_path
            FROM node n
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
            JOIN category c ON c.cid = n.nid AND c.cnid = 12622
            LEFT JOIN (
            SELECT 12625 nameable_id, 'adoption agencies' topic_name
            UNION
            SELECT 35715, 'church'
            UNION
            SELECT 31716, 'boot camp'
            UNION
            SELECT 12632, 'media'
            UNION
            SELECT 48309, 'blog'
            UNION
            SELECT 12635, 'orphanages'
            UNION
            SELECT 12624, 'Adoption Advocates'
            UNION
            SELECT 31586, 'boarding school'
            UNION
            SELECT 14670, 'adoption facilitators'
            UNION
            SELECT 17310, 'maternity homes'
            ) v ON v.nameable_id = n.nid
            LEFT JOIN (
            	SELECT 
            	n2.nid,
            	n2.title,
            	nr2.body,
            	cc.field_tile_image_fid,
            	GROUP_CONCAT(n3.title, ',') parent_topics
            	FROM node n2 
            	JOIN category c ON c.cid = n2.nid AND c.cnid = 4126
            	JOIN content_type_category_cat cc ON cc.vid = n2.vid AND cc.nid = n2.nid
            	JOIN node_revisions nr2 ON nr2.nid = n2.nid AND nr2.vid = n2.vid
            	LEFT JOIN category_hierarchy ch ON ch.cid = n2.nid
            	LEFT JOIN node n3 ON n3.nid = ch.parent
            	WHERE n2.`type` = 'category_cat' AND n3.title <> 'Topics'
            	GROUP BY
            			n2.nid,
            		n2.title,
            		nr2.body,
            		cc.field_tile_image_fid

            ) n2 ON n2.title = v.topic_name
            WHERE n.nid NOT IN (38308, 38518)
            """;

        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        var vocabularyIdTopics = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });
        var vocabularyIdOrganizationType = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_ORGANIZATION_TYPE
        });


        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var topicName = reader.GetString("topic_name");

            var topicParentNames = reader.GetString("topic_parent_names")
                    .Split(',')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
            
            List<int> topicParentIds = new List<int>();
            foreach (var topicParentName in topicParentNames) {
                topicParentIds.Add(await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                    Name = topicParentName,
                    VocabularyId = vocabularyIdTopics
                }));
            }
            var vocabularyNames = new List<NewTermForNewNameble>
            {
                new NewTermForNewNameble
                {
                    VocabularyId = vocabularyIdOrganizationType,
                    Name = name,
                    ParentTermIds = new List<int>()
                },
                new NewTermForNewNameble {
                    VocabularyId = vocabularyIdTopics,
                    Name = topicName,
                    ParentTermIds = topicParentIds,
                }
            };

            yield return new NewOrganizationType {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                OwnerId = Constants.OWNER_PARTIES,
                AuthoringStatusId = 1,
                TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        SubgroupId = null,
                        UrlId = id
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = id < 33163 ? id : null
                    }
                },
                NodeTypeId = 1,
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image")
                    ? null
                    : await fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileIdRequest {
                        TenantId = Constants.PPL,
                        TenantFileId = reader.GetInt32("file_id_tile_image")
                    }),
                Terms = vocabularyNames,
                HasConcreteSubtype = reader.GetBoolean("has_concrete_subtype"),
                NodeTermIds = new List<int>(),
            };
        }
        reader.Close();

        var now = DateTime.Now;
        yield return new NewOrganizationType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.POLITICAL_PARTY_NAME,
            OwnerId = Constants.OWNER_PARTIES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
               {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.POLITICAL_PARTY
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.POLITICAL_PARTY
                    }
                },
            NodeTypeId = 1,
            Description = "",
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameble>
            {
                new NewTermForNewNameble
                {
                    VocabularyId = vocabularyIdOrganizationType,
                    Name = Constants.POLITICAL_PARTY_NAME,
                    ParentTermIds = new List<int>(),
                },
                new NewTermForNewNameble {
                    VocabularyId = vocabularyIdTopics,
                    Name = Constants.POLITICAL_PARTY_NAME.ToLower(),
                    ParentTermIds = new List<int>{
                        await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                            Name = "organizations",
                            VocabularyId = vocabularyIdTopics
                        })
                    },
                }
            },
            HasConcreteSubtype = true,
            NodeTermIds = new List<int>(),
        };
    }
}
