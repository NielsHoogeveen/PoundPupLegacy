namespace PoundPupLegacy.Convert;

internal sealed class BasicNameableMigrator : MigratorPPL
{
    private readonly IEntityCreator<BasicNameable> _basicNameableCreator;
    public BasicNameableMigrator(
        IDatabaseConnections databaseConnections,
        IEntityCreator<BasicNameable> basicNameableCreator
    ) : base(databaseConnections)
    {
        _basicNameableCreator = basicNameableCreator;
    }

    protected override string Name => "basic nameables";

    protected override async Task MigrateImpl()
    {
        await _basicNameableCreator.CreateAsync(ReadBasicNameables(), _postgresConnection);
    }
    private async IAsyncEnumerable<BasicNameable> ReadBasicNameables()
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    41 node_type_id,
                    nr.body description,
                    n2.`type`,
                case 
                    when field_tile_image_fid = 0 then null
                    ELSE field_tile_image_fid
                END file_id_tile_image,
                ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN category c ON c.cid = n.nid AND c.cnid = 4126
                JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                LEFT JOIN node n2 ON n2.title = n.title AND n2.nid <> n.nid AND n2.`type` IN ('adopt_person','country_type', 'adopt_orgs', 'case', 'region_facts', 'coerced_adoption_cases', 'child_trafficking', 'child_trafficking_case')
                LEFT JOIN node n3 ON n3.nid = cc.field_related_page_nid
                WHERE  (n3.nid IS NULL OR n3.`type` = 'group')
                AND n.title NOT IN 
                (
                    'adoption',
                    'adoptive mother',
                    'foster care',
                    'guardianship',
                    'institutional care',
                    'adoption agencies',
                    'legal guardians',
                    'church',
                    'boot camp',
                    'media',
                    'blog',
                    'orphanages',
                    'orphanage',
                    'adoption advocates',
                    'boarding school',
                    'adoption facilitators',
                    'maternity homes',
                    'sexual abuse',
                    'sexual exploitation',
                    'lethal neglect',
                    'lethal deprivation',
                    'economic exploitation',
                    'verbal abuse',
                    'medical abuse',
                    'lawyers',
                    'therapists',
                    'Christianity',
                    'Northern Ireland',
                    'mega families',
                    'Mary Landrieu',
                    'Hilary Clinton',
                    'Michele Bachmann',
                    'Kevin and Kody Pribbernow'
                )
                AND n.nid NOT IN (
                    22589, 74730, 27064, 45581, 52679, 46859, 30753, 14781, 28576, 37627, 48018, 19559
                )
                AND n2.nid IS  NULL
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        yield return new BasicNameable {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "organizations",
            OwnerId = 1,
            AuthoringStatusId = 1,
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
                    UrlId = null
                }
            },
            NodeTypeId = 41,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = "organizations",
                    ParentNames = new List<string>(),
                }
            },
        };

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var parentNames = id == 4170
                ? new List<string> { "organizations" }
                : new List<string>();
            var vocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.OWNER_SYSTEM,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = name,
                        ParentNames = parentNames,
                    }
                };

            yield return new BasicNameable {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = name,
                OwnerId = 1,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("status"),
                        UrlPath = reader.IsDBNull("url_path") ? null: reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = reader.GetInt32("node_type_id"),
                Description = reader.GetString("description"),
                FileIdTileImage = null,
                VocabularyNames = vocabularyNames,
            };

        }
        await reader.CloseAsync();

        yield return new BasicNameable {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "US senate bill",
            OwnerId = 1,
            AuthoringStatusId = 1,
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
                    UrlId = null
                }
            },
            NodeTypeId = 41,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = "US senate bill",
                    ParentNames = new List<string>{"United States Congress"},
                }
            },
        };
        yield return new BasicNameable {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "US house bill",
            OwnerId = 1,
            AuthoringStatusId = 1,
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
                    UrlId = null
                }
            },
            NodeTypeId = 41,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = "US house bill",
                    ParentNames = new List<string>{"United States Congress"},
                }
            },
        };
        yield return new BasicNameable {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "US act",
            OwnerId = 1,
            AuthoringStatusId = 1,
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
                    UrlId = null
                }
            },
            NodeTypeId = 41,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = "US act",
                    ParentNames = new List<string>{"United States Congress"},
                }
            },
        };
    }
}
