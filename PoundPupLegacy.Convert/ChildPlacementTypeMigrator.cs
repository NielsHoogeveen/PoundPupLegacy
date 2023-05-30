namespace PoundPupLegacy.Convert;

internal sealed class ChildPlacementTypeMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileIdFactory,
    IEntityCreatorFactory<ChildPlacementType.ToCreate> childPlacementTypeCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "child placement types";

    protected override async Task MigrateImpl()
    {
        await using var fileIdReaderByTenantFileId = await fileIdReaderByTenantFileIdFactory.CreateAsync(_postgresConnection);
        await using var childPlacementTypeCreator = await childPlacementTypeCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await childPlacementTypeCreator.CreateAsync(ReadChildPlacementTypes(nodeIdReader,termIdReader,fileIdReaderByTenantFileId));
    }

    private async IAsyncEnumerable<ChildPlacementType.ToCreate> ReadChildPlacementTypes(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        IMandatorySingleItemDatabaseReader<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileId)
    {
        var sql = $"""
            SELECT
            t.id,
            1 access_role_id,
            NOW() created_date_time,
            NOW() changed_date_time,
            t.title,
            1 node_status_id,
            27 node_type_id,
            case 
                when nr.body IS NULL then ''
                ELSE nr.body
                	end description,
            t.topic_name,
            t.parent_topic_name,
            n.nid,
            case 
                when cc.field_tile_image_fid = 0 then null
                ELSE cc.field_tile_image_fid 
            end file_id_tile_image,
            ua.dst url_path
            FROM(
            SELECT {Constants.ADOPTION} AS id, 'Adoption' AS title, 'adoption' AS topic_name, 'Child placement forms' AS parent_topic_name
            UNION
            SELECT {Constants.FOSTER_CARE}, 'Foster Care', 'foster care', 'Child placement forms'
            UNION
            SELECT {Constants.TO_BE_ADOPTED}, 'To be adopted', NULL, NULL
            UNION
            SELECT {Constants.LEGAL_GUARDIANSHIP}, 'Legal Guardianship', 'guardianship', 'Child placement forms'
            UNION
            SELECT {Constants.INSTITUTION}, 'Institution', 'institutional care', 'residential care'
            ) AS t
            LEFT JOIN node n ON n.title = t.topic_name AND n.`type` = 'category_cat'
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            LEFT JOIN category c ON c.cid = n.nid AND c.cnid = 4126
            LEFT JOIN content_type_category_cat cc on cc.nid = n.nid AND cc.vid = n.vid
            LEFT JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
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
        var vocabularyIdChildPlacementType = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_CHILD_PLACEMENT_TYPE
        });

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var topicName = reader.IsDBNull("topic_name") ? null : reader.GetString("topic_name");
            var topicParentName = reader.IsDBNull("parent_topic_name") ? null : reader.GetString("parent_topic_name");
            var vocabularyNames = new List<NewTermForNewNameable>
            {
                new NewTermForNewNameable
                {
                    Identification = new Identification.Possible {
                        Id = null,
                    },
                    VocabularyId = vocabularyIdChildPlacementType,
                    Name = name,
                    ParentTermIds = new List<int>(),
                }
            };
            if (topicName != null) {
                var parentTermIds = new List<int>();
                if (topicParentName != null) {
                    parentTermIds.Add(await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                        Name = topicParentName,
                        VocabularyId = vocabularyIdTopics
                    }));
                }
                vocabularyNames.Add(new NewTermForNewNameable {
                    Identification = new Identification.Possible {
                        Id = null,
                    },
                    VocabularyId = vocabularyIdTopics,
                    Name = topicName,
                    ParentTermIds = parentTermIds
                });
            }

            yield return new ChildPlacementType.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.ForCreate {
                    PublisherId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    OwnerId = Constants.OWNER_CASES,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreateForNewNode>
                    {
                        new TenantNode.ToCreateForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.PPL,
                            PublicationStatusId = reader.GetInt32("node_status_id"),
                            UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                            SubgroupId = null,
                            UrlId = id
                        },
                        new TenantNode.ToCreateForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.CPCT,
                            PublicationStatusId = 2,
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = id < 33163 ? id : null
                        }
                    },
                    NodeTypeId = reader.GetInt32("node_type_id"),
                    TermIds = new List<int>(),
                },
                NameableDetails = new NameableDetails.ForCreate {
                    Description = reader.GetString("description"),
                    FileIdTileImage = reader.IsDBNull("file_id_tile_image")
                    ? null
                    : await fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileIdRequest {
                        TenantId = Constants.PPL,
                        TenantFileId = reader.GetInt32("file_id_tile_image")
                    }),
                    Terms = vocabularyNames,
                },
            };
        }
        await reader.CloseAsync();
    }
}
