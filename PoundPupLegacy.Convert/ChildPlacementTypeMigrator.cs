using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class ChildPlacementTypeMigrator : PPLMigrator
{

    public ChildPlacementTypeMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }

    protected override string Name => "child placement types";

    protected override async Task MigrateImpl()
    {
        await ChildPlacementTypeCreator.CreateAsync(ReadChildPlacementTypes(), _postgresConnection);
    }

    private async IAsyncEnumerable<ChildPlacementType> ReadChildPlacementTypes()
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
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var topicName = reader.IsDBNull("topic_name") ? null : reader.GetString("topic_name");
            var parentTopicName = reader.IsDBNull("parent_topic_name") ? null : reader.GetString("parent_topic_name");
            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_CASES,
                    Name = Constants.VOCABULARY_CHILD_PLACEMENT_TYPE,
                    TermName = name,
                    ParentNames = new List<string>(),
                }
            };
            if (topicName != null) {
                var parentNames = new List<string>();
                if (parentTopicName != null) {
                    parentNames.Add(parentTopicName);
                }
                vocabularyNames.Add(new VocabularyName {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = topicName,
                    ParentNames = parentNames
                });
            }

            yield return new ChildPlacementType {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                OwnerId = Constants.OWNER_CASES,
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
                NodeTypeId = reader.GetInt32("node_type_id"),
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image") 
                    ? null 
                    : await _fileIdReaderByTenantFileId.ReadAsync(new Db.Readers.FileIdReaderByTenantFileId.FileIdReaderByTenantFileIdRequest 
                    { 
                        TenantId = Constants.PPL,
                        TenantFileId = reader.GetInt32("file_id_tile_image")
                    }),
                VocabularyNames = vocabularyNames,
            };

        }
        await reader.CloseAsync();
    }

}
