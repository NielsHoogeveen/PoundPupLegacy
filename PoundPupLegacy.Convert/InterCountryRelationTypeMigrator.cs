using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class InterCountryRelationTypeMigrator : PPLMigrator
{

    public InterCountryRelationTypeMigrator(MySqlToPostgresConverter converter) : base(converter) { }

    protected override string Name => "inter-country relation types";

    protected override async Task MigrateImpl()
    {
        await InterCountryRelationTypeCreator.CreateAsync(ReadInterCountryRelationTypes(), _postgresConnection);
    }
    private async IAsyncEnumerable<InterCountryRelationType> ReadInterCountryRelationTypes()
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

        using var readCommand = MysqlConnection.CreateCommand();
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
                FileIdTileImage = reader.IsDBNull("file_id_tile_image") ? null : await _fileIdReaderByTenantFileId.ReadAsync(Constants.PPL, reader.GetInt32("file_id_tile_image")),
                VocabularyNames = vocabularyNames,
                IsSymmetric = reader.GetBoolean("is_symmetric"),
            };

        }
        await reader.CloseAsync();
    }
}
