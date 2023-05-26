namespace PoundPupLegacy.Convert;

internal sealed class BasicNameableMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
    IEntityCreatorFactory<EventuallyIdentifiableBasicNameable> basicNameableCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "basic nameables";

    protected override async Task MigrateImpl()
    {
        await using var basicNameableCreator = await basicNameableCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await basicNameableCreator.CreateAsync(ReadBasicNameables(nodeIdReader,termIdReader));
    }
    private async IAsyncEnumerable<NewBasicNameable> ReadBasicNameables(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader)
    {
        var vocabularyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

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

        yield return new NewBasicNameable {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "organizations",
            OwnerId = 1,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
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
                    VocabularyId = vocabularyId,
                    TermName = "organizations",
                    ParentTermIds = new List<int>(),
                }
            },
            NodeTermIds = new List<int>(),
        };

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var parentNames = id == 4170
                ? new List<string> { "organizations" }
                : new List<string>();
            List<int> topicParentIds = new List<int>();
            foreach (var parentName in parentNames) {
                topicParentIds.Add(await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                    Name = parentName,
                    VocabularyId = vocabularyId
                }));
            }
            var vocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        VocabularyId = vocabularyId,
                        TermName = name,
                        ParentTermIds = topicParentIds,
                    }
                };

            yield return new NewBasicNameable {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = name,
                OwnerId = 1,
                AuthoringStatusId = 1,
                TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
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
                NodeTermIds = new List<int>(),
            };

        }
        await reader.CloseAsync();

        yield return new NewBasicNameable {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "US senate bill",
            OwnerId = 1,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
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
                    VocabularyId = vocabularyId,
                    TermName = "US senate bill",
                    ParentTermIds = new List<int>{
                        await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                            Name = "United States Congress",
                            VocabularyId = vocabularyId
                        })
                    },
                }
            },
            NodeTermIds = new List<int>(),
        };
        yield return new NewBasicNameable {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "US house bill",
            OwnerId = 1,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
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
                    VocabularyId = vocabularyId,
                    TermName = "US house bill",
                    ParentTermIds = new List<int>{
                        await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                            Name = "United States Congress",
                            VocabularyId = vocabularyId
                        })
                    },
                }
            },
            NodeTermIds = new List<int>(),
        };
        yield return new NewBasicNameable {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "US act",
            OwnerId = 1,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
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
                    VocabularyId = vocabularyId,
                    TermName = "US act",
                    ParentTermIds = new List<int>{
                        await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                            Name = "United States Congress",
                            VocabularyId = vocabularyId
                        })
                    },
                }
            },
            NodeTermIds = new List<int>(),
        };
    }
}
