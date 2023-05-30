namespace PoundPupLegacy.Convert;

internal sealed class FirstLevelGlobalRegionMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileIdFactory,
    IEntityCreatorFactory<FirstLevelGlobalRegion.ToCreate> firstLevelGlobalRegionCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "first level global regions";

    protected override async Task MigrateImpl()
    {
        await using var fileIdReaderByTenantFileId = await fileIdReaderByTenantFileIdFactory.CreateAsync(_postgresConnection);
        await using var firstLevelGlobalRegionCreator = await firstLevelGlobalRegionCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await firstLevelGlobalRegionCreator.CreateAsync(ReadFirstLevelGlobalRegions(nodeIdReader,fileIdReaderByTenantFileId));
    }

    private async IAsyncEnumerable<FirstLevelGlobalRegion.ToCreate> ReadFirstLevelGlobalRegions(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileId)
    {
        var sql = $"""
            SELECT n.nid id,
            n.uid access_role_id,
            n.title,
            n.`status` node_status_id,
            FROM_UNIXTIME(n.created) created_date_time, 
            FROM_UNIXTIME(n.changed) changed_date_time,
            nr.body description,
            cc.field_tile_image_fid file_id_tile_image,
            ua.dst url_path
            FROM node n 
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            JOIN category_hierarchy ch ON ch.cid = n.nid
            JOIN node n2 ON n2.nid = ch.parent
                	LEFT JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                LEFT JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
            WHERE n.`type` = 'region_facts'
            AND n.nid < 30000
            AND n2.`type` = 'category_cont'
            """;

        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();
        var vocabularyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");

            var vocabularyNames = new List<NewTermForNewNameable>
            {
                new NewTermForNewNameable
                {
                    IdentificationForCreate = new Identification.Possible {
                        Id = null,
                    },
                    VocabularyId = vocabularyId,
                    Name = name,
                    ParentTermIds = new List<int>(),
                }
            };

            yield return new FirstLevelGlobalRegion.ToCreate {
                IdentificationForCreate = new Identification.Possible {
                    Id = null
                },
                NodeDetailsForCreate =new NodeDetails.NodeDetailsForCreate {
                    PublisherId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    OwnerId = Constants.OWNER_GEOGRAPHY,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreateForNewNode>
                    {
                        new TenantNode.ToCreateForNewNode
                        {
                            IdentificationForCreate = new Identification.Possible {
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
                            IdentificationForCreate = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.CPCT,
                            PublicationStatusId = 2,
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = id < 33163 ? id : null
                        }
                    },
                    NodeTypeId = 11,
                    TermIds = new List<int>(),
                },
                NameableDetailsForCreate = new NameableDetails.NameableDetailsForCreate {
                    Terms = vocabularyNames,
                    Description = reader.GetString("description"),
                    FileIdTileImage = reader.IsDBNull("file_id_tile_image")
                    ? null
                    : await fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileIdRequest {
                        TenantId = Constants.PPL,
                        TenantFileId = reader.GetInt32("file_id_tile_image")
                    }),
                },
                GlobalRegionDetails =new GlobalRegionDetails {
                    Name = name,
                },
            };
        }
        await reader.CloseAsync();
    }
}

