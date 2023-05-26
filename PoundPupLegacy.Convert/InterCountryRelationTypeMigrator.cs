namespace PoundPupLegacy.Convert;

internal sealed class InterCountryRelationTypeMigrator(
IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileIdFactory,
    IEntityCreatorFactory<EventuallyIdentifiableInterCountryRelationType> interCountryRelationTypeCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "inter-country relation types";

    protected override async Task MigrateImpl()
    {
        await using var fileIdReaderByTenantFileId = await fileIdReaderByTenantFileIdFactory.CreateAsync(_postgresConnection);
        await using var interCountryRelationTypeCreator = await interCountryRelationTypeCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await interCountryRelationTypeCreator.CreateAsync(ReadInterCountryRelationTypes(nodeIdReader,fileIdReaderByTenantFileId));
    }
    private async IAsyncEnumerable<NewInterCountryRelationType> ReadInterCountryRelationTypes(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileId)
    {

        var sql = $"""
                SELECT
                    id,
                    1 access_role_id,
                    title,
                    1 node_status_id,
                    NOW() created_date_time, 
                    NOW() changed_date_time,
                    '' description,
                    NULL file_id_tile_image,
                 	0 is_symmetric,
                    NULL url_path
                FROM (
                	SELECT {Constants.ADOPTION_IMPORT} id, 'imports from' title
                	UNION
                	SELECT {Constants.ADOPTION_EXPORT} id, 'exports to' title
                ) x
                """;

        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();
        var vocabularyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_INTER_PERSONAL_RELATION_TYPE
        });

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");

            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    VocabularyId = vocabularyId,
                    TermName = name,
                    ParentTermIds = new List<int>(),
                }
            };
            yield return new NewInterCountryRelationType {
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
                NodeTypeId = 50,
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image")
                    ? null
                    : await fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileIdRequest {
                        TenantId = Constants.PPL,
                        TenantFileId = reader.GetInt32("file_id_tile_image")
                    }),
                VocabularyNames = vocabularyNames,
                IsSymmetric = reader.GetBoolean("is_symmetric"),
                NodeTermIds = new List<int>(),
            };

        }
        await reader.CloseAsync();
    }
}
