namespace PoundPupLegacy.Convert;

internal sealed class OrganizationTypeMigrator : PPLMigrator
{

    public OrganizationTypeMigrator(MySqlToPostgresConverter converter) : base(converter) { }

    protected override string Name => "organization types";

    protected override async Task MigrateImpl()
    {
        await new OrganizationTypeCreator().CreateAsync(ReadOrganizationTypes(), _postgresConnection);
    }
    private async IAsyncEnumerable<OrganizationType> ReadOrganizationTypes()
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
                n2.title topic_name,
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
            cc.field_tile_image_fid
            FROM node n2 
            JOIN category c ON c.cid = n2.nid AND c.cnid = 4126
            JOIN content_type_category_cat cc ON cc.vid = n2.vid AND cc.nid = n2.nid
            JOIN node_revisions nr2 ON nr2.nid = n2.nid AND nr2.vid = n2.vid
            WHERE n2.`type` = 'category_cat'
            ) n2 ON n2.title = v.topic_name
            """;

        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var topicName = reader.IsDBNull("topic_name") ? null : reader.GetString("topic_name");

            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_PARTIES,
                    Name = Constants.VOCABULARY_ORGANIZATION_TYPE,
                    TermName = name,
                    ParentNames = new List<string>(),
                }
            };
            if (topicName != null) {
                vocabularyNames.Add(new VocabularyName {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = topicName,
                    ParentNames = new List<string>()
                });
            }

            yield return new OrganizationType {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                OwnerId = Constants.OWNER_PARTIES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id < 33163 ? id : null
                    }
                },
                NodeTypeId = 1,
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image")
                    ? null
                    : await _fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileId.Request {
                        TenantId = Constants.PPL,
                        TenantFileId = reader.GetInt32("file_id_tile_image")
                    }),
                VocabularyNames = vocabularyNames,
                HasConcreteSubtype = reader.GetBoolean("has_concrete_subtype"),
            };
        }
        reader.Close();

        var now = DateTime.Now;
        yield return new OrganizationType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.POLITICAL_PARTY_NAME,
            OwnerId = Constants.OWNER_PARTIES,
            TenantNodes = new List<TenantNode>
               {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.POLITICAL_PARTY
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.POLITICAL_PARTY
                    }
                },
            NodeTypeId = 1,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_PARTIES,
                    Name = Constants.VOCABULARY_ORGANIZATION_TYPE,
                    TermName = Constants.POLITICAL_PARTY_NAME,
                    ParentNames = new List<string>(),
                }
            },
            HasConcreteSubtype = true,
        };
    }
}
