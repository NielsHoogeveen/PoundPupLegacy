
namespace PoundPupLegacy.Convert;

internal sealed class InterCountryRelationTypeMigrator : MigratorPPL
{
    private readonly IDatabaseReaderFactory<FileIdReaderByTenantFileId> _fileIdReaderByTenantFileIdFactory;
    private readonly IEntityCreator<InterCountryRelationType> _interCountryRelationTypeCreator;
    public InterCountryRelationTypeMigrator(
    IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<FileIdReaderByTenantFileId> fileIdReaderByTenantFileIdFactory,
        IEntityCreator<InterCountryRelationType> interCountryRelationTypeCreator
    ) : base(databaseConnections)
    {
        _fileIdReaderByTenantFileIdFactory = fileIdReaderByTenantFileIdFactory;
        _interCountryRelationTypeCreator = interCountryRelationTypeCreator;
    }

    protected override string Name => "inter-country relation types";

    protected override async Task MigrateImpl()
    {
        await using var fileIdReaderByTenantFileId = await _fileIdReaderByTenantFileIdFactory.CreateAsync(_postgresConnection);

        await _interCountryRelationTypeCreator.CreateAsync(ReadInterCountryRelationTypes(fileIdReaderByTenantFileId), _postgresConnection);
    }
    private async IAsyncEnumerable<InterCountryRelationType> ReadInterCountryRelationTypes(FileIdReaderByTenantFileId fileIdReaderByTenantFileId)
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

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");

            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_PARTIES,
                    Name = Constants.VOCABULARY_INTERPERSONAL_RELATION_TYPE,
                    TermName = name,
                    ParentNames = new List<string>(),
                }
            };
            yield return new InterCountryRelationType {
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
                NodeTypeId = 50,
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image")
                    ? null
                    : await fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileId.Request {
                        TenantId = Constants.PPL,
                        TenantFileId = reader.GetInt32("file_id_tile_image")
                    }),
                VocabularyNames = vocabularyNames,
                IsSymmetric = reader.GetBoolean("is_symmetric"),
            };

        }
        await reader.CloseAsync();
    }
}
