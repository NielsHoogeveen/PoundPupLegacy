namespace PoundPupLegacy.Convert;

internal sealed class DenominationMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileIdFactory,
    IEntityCreator<Denomination> denominationCreator
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "denominations";

    protected override async Task MigrateImpl()
    {
        await using var fileIdReaderByTenantFileId = await fileIdReaderByTenantFileIdFactory.CreateAsync(_postgresConnection);
        await denominationCreator.CreateAsync(ReadDenominations(fileIdReaderByTenantFileId), _postgresConnection);
    }
    private async IAsyncEnumerable<Denomination> ReadDenominations(
        IMandatorySingleItemDatabaseReader<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileId)
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
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                JOIN category c ON c.cid = n.nid AND c.cnid = 39428
                LEFT JOIN (
                SELECT 39429 nameable_id, 'Christianity' topic_name
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

        using var readCommand = _mySqlConnection.CreateCommand();
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
                    Name = Constants.VOCABULARY_DENOMINATION,
                    TermName = name,
                    ParentNames = new List<string>(),
                }
            };
            if (topicName != null) {
                vocabularyNames.Add(new VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = topicName,
                    ParentNames = new List<string>()
                });
            }

            yield return new Denomination {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                OwnerId = Constants.OWNER_PARTIES,
                AuthoringStatusId = 1,
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
                NodeTypeId = 7,
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image")
                    ? null
                    : await fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileIdRequest {
                        TenantId = Constants.PPL,
                        TenantFileId = reader.GetInt32("file_id_tile_image")
                    }),
                VocabularyNames = vocabularyNames,
            };
        }
        reader.Close();
    }
}
