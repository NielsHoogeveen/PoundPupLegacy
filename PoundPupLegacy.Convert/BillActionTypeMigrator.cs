namespace PoundPupLegacy.Convert;

internal sealed class BillActionTypeMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileIdFactory,
        INameableCreatorFactory<EventuallyIdentifiableBillActionType> billActionTypeCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "person organization relation types";
    protected override async Task MigrateImpl()
    {
        await using var fileIdReaderByTenantFileId = await fileIdReaderByTenantFileIdFactory.CreateAsync(_postgresConnection);
        await using var billActionTypeCreator = await billActionTypeCreatorFactory.CreateAsync(_postgresConnection);
        await billActionTypeCreator.CreateAsync(ReadBillActionTypes(fileIdReaderByTenantFileId));
    }
    private async IAsyncEnumerable<NewBillActionType> ReadBillActionTypes(
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
                    '' description,
                    NULL file_id_tile_image,
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                JOIN category c ON c.cid = n.nid AND c.cnid = 12663
                WHERE n.nid in (38509, 38312)
                """;

        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");

            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_PARTIES,
                    Name = Constants.VOCABULARY_PERSON_ORGANIZATION_RELATION_TYPE,
                    TermName = name,
                    ParentNames = new List<string>(),
                }
            };

            yield return new NewBillActionType {
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
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    },
                    new NewTenantNodeForNewNode
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
                NodeTypeId = 58,
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image")
                    ? null
                    : await fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileIdRequest {
                        TenantId = Constants.PPL,
                        TenantFileId = reader.GetInt32("file_id_tile_image")
                    }),
                VocabularyNames = vocabularyNames,
                NodeTermIds = new List<int>(),
            };

        }
        await reader.CloseAsync();
    }
}
