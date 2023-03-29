namespace PoundPupLegacy.Convert;

internal sealed class ProfessionMigrator : PPLMigrator
{

    public ProfessionMigrator(MySqlToPostgresConverter converter) : base(converter) { }

    protected override string Name => "professions";

    protected override async Task MigrateImpl()
    {
        await ProfessionCreator.CreateAsync(ReadProfessions(), _postgresConnection);
    }
    private async IAsyncEnumerable<Profession> ReadProfessions()
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
                    when n.nid IN (27214,27215) then true
                    ELSE false
                    END has_concrete_subtype,
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                JOIN category c ON c.cid = n.nid AND c.cnid = 27213
                LEFT JOIN (
                SELECT 27214 nameable_id, 'lawyers' topic_name
                UNION
                SELECT 27215, 'therapists'
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
                    Name = Constants.VOCABULARY_PROFESSION,
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

            yield return new Profession {
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
                        TenantId = 1,
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
                NodeTypeId = 6,
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image")
                    ? null
                    : await _fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileId.FileIdReaderByTenantFileIdRequest {
                        TenantId = Constants.PPL,
                        TenantFileId = reader.GetInt32("file_id_tile_image")
                    }),
                VocabularyNames = vocabularyNames,
                HasConcreteSubtype = reader.GetBoolean("has_concrete_subtype"),
            };
        }
        reader.Close();
        yield return new Profession {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "Senator",
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
                        UrlId = null
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = null
                    }
                },
            NodeTypeId = 6,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_PARTIES,
                    Name = Constants.VOCABULARY_PROFESSION,
                    TermName = "Senator",
                    ParentNames = new List<string>(),
                }
            },
            HasConcreteSubtype = true,

        };
        yield return new Profession {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "Representative",
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
                        UrlId = null
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = null
                    }
                },
            NodeTypeId = 6,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_PARTIES,
                    Name = Constants.VOCABULARY_PROFESSION,
                    TermName = "Representative",
                    ParentNames = new List<string>(),
                }
            },
            HasConcreteSubtype = true,

        };

    }
}
